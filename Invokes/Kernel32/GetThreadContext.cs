using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class GetThreadContext : IInvoke
    {
        string IInvoke.Name => "GetThreadContext";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
