using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class WriteProcessMemory_ByteArray : IInvoke
    {
        string IInvoke.Name => "WriteProcessMemory_ByteArray";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, uint nSize,
            out IntPtr lpNumberOfBytesWritten);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
