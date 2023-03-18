using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadpoolWait : IInvoke
    {
        string IInvoke.Name => "SetThreadpoolWait";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr SetThreadpoolWait(
            IntPtr pwa, IntPtr h, IntPtr pftTimeout);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
