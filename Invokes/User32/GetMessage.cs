using System;

namespace SingleDose.Invokes.User32
{
    internal class GetMessage : IInvoke
    {
        string IInvoke.Name => "GetMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern int GetMessage(
            out MSG lpMsg, IntPtr hWnd, 
            uint wMsgFilterMin, uint wMsgFilterMax);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
