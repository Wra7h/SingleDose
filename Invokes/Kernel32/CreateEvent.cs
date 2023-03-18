using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateEvent : IInvoke
    {
        string IInvoke.Name => "CreateEvent";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes, bool bManualReset,
            bool bInitialState, string lpName);
            
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
