using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CloseThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "CloseThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolTimer(IntPtr pti);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
