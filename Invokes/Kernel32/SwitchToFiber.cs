using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SwitchToFiber : IInvoke
    {
        string IInvoke.Name => "SwitchToFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        extern static IntPtr SwitchToFiber(IntPtr lpFiber);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
