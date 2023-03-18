using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThread : IInvoke
    {
        string IInvoke.Name => "CreateThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"", CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateThread(
            IntPtr lpThreadAttributes, uint dwStackSize,
            IntPtr lpStartAddress, IntPtr lpParameter,
            uint dwCreationFlags, IntPtr lpThreadId);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
