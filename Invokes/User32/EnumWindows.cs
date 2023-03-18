using System;

namespace SingleDose.Invokes.User32
{
    internal class EnumWindows : IInvoke
    {
        string IInvoke.Name => "EnumWindows";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        static extern bool EnumWindows(
            IntPtr lpEnumFunc, IntPtr lParam);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
