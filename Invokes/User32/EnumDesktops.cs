using System;

namespace SingleDose.Invokes.User32
{
    internal class EnumDesktops : IInvoke
    {
        string IInvoke.Name => "EnumDesktops";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        static extern bool EnumDesktops(
            IntPtr hwinsta, IntPtr lpEnumFunc,
            IntPtr lParam);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
