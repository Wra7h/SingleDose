using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "SetThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void SetThreadpoolTimer(
            IntPtr pti, ref FILETIME pv,
            uint msPeriod, uint msWindowLength);
            
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
