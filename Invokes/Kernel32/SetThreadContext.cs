using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadContext : IInvoke
    {
        string IInvoke.Name => "SetThreadContext";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
