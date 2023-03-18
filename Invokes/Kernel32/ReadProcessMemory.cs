using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class ReadProcessMemory : IInvoke
    {
        string IInvoke.Name => "ReadProcessMemory";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess, IntPtr lpBaseAddress,
            byte[] lpBuffer, Int32 nSize,
            out IntPtr lpNumberOfBytesRead);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
