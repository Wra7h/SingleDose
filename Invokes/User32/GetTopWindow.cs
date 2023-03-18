using System;

namespace SingleDose.Invokes.User32
{
    internal class GetTopWindow : IInvoke
    {
        string IInvoke.Name => "GetTopWindow";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
