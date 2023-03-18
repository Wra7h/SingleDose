using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateWaitableTimer : IInvoke
    {
        string IInvoke.Name => "CreateWaitableTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateWaitableTimer(
            IntPtr lpTimerAttributes, bool bManualReset,
            string lpTimerName);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
