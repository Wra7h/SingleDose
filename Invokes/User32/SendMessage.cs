using System;

namespace SingleDose.Invokes.User32
{
    internal class SendMessage : IInvoke
    {
        string IInvoke.Name => "SendMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern int SendMessage(
            IntPtr hWnd, uint Msg,
            IntPtr wParam, ref COPYDATASTRUCT lParam);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
