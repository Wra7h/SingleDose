using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class FlsAlloc : IInvoke
    {
        string IInvoke.Name => "FlsAlloc";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint FlsAlloc(IntPtr callback);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
