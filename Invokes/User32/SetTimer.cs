using System;

namespace SingleDose.Invokes.User32
{
    internal class SetTimer : IInvoke
    {
        string IInvoke.Name => "SetTimer";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"", ExactSpelling = true)]
        static extern IntPtr SetTimer(
            IntPtr hWnd, IntPtr nIDEvent, 
            uint uElapse, IntPtr lpTimerFunc);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
