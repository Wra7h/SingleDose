using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolWait : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolWait";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWait(
            IntPtr pfnwa, IntPtr pv, IntPtr pcbe);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
