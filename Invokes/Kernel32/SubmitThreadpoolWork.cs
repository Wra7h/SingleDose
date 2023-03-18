using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SubmitThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "SubmitThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void SubmitThreadpoolWork(
            IntPtr pwkl);

        {{PINVOKE}}";
        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
