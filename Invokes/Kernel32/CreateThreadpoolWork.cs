using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWork(
            IntPtr pfnwk, IntPtr pv, IntPtr pcbe);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
