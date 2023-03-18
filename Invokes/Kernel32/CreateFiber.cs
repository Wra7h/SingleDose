using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateFiber : IInvoke
    {
        string IInvoke.Name => "CreateFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFiber(
            uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
