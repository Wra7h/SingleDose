using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForThreadpoolWorkCallbacks : IInvoke
    {
        string IInvoke.Name => "WaitForThreadpoolWorkCallbacks";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolWorkCallbacks(
            IntPtr pwk, bool fCancelPendingCallbacks);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
