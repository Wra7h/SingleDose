using System;

namespace SingleDose.Invokes.User32
{
    internal class DispatchMessage : IInvoke
    {
        string IInvoke.Name => "DispatchMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
