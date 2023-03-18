using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolTimer(
            IntPtr pfnti, IntPtr pv, IntPtr pcbe);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
