using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class ConvertThreadToFiber : IInvoke
    {
        string IInvoke.Name => "ConvertThreadToFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr ConvertThreadToFiber(IntPtr lpParameter);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
