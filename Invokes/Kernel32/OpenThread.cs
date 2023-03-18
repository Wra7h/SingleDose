using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class OpenThread : IInvoke
    {
        string IInvoke.Name => "OpenThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
