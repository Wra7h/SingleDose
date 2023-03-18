using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CloseThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "CloseThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolWork(IntPtr pwk);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
