using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class WriteProcessMemory_IntPtr : IInvoke
    {
        string IInvoke.Name => "WriteProcessMemory_IntPtr";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool WriteProcessMemory(
             IntPtr hProcess, IntPtr lpBaseAddress,
             IntPtr lpBuffer, Int32 nSize,
             out IntPtr lpNumberOfBytesWritten);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
