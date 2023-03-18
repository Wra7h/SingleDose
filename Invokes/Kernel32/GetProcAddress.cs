using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class GetProcAddress : IInvoke
    {
        string IInvoke.Name => "GetProcAddress";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
