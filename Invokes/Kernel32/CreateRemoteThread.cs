using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateRemoteThread : IInvoke
    {
        string IInvoke.Name => "CreateRemoteThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateRemoteThread(
            IntPtr hProcess, IntPtr lpThreadAttributes,
            uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter, uint dwCreationFlags,
            IntPtr lpThreadId);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
