using System;

namespace SingleDose.Invokes.User32
{
    internal class EnumChildWindows : IInvoke
    {
        string IInvoke.Name => "EnumChildWindows";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        public static extern bool EnumChildWindows(
            IntPtr hWndParent, IntPtr lpEnumFunc,
            IntPtr lParam);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
