using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForThreadpoolTimerCallbacks : IInvoke
    {
        string IInvoke.Name => "WaitForThreadpoolTimerCallbacks";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolTimerCallbacks(
            IntPtr pti, bool fCancelPendingCallbacks);

        {{PINVOKE}}";
        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
