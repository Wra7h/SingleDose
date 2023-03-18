using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class VirtualAllocEx : IInvoke
    {
        string IInvoke.Name => "VirtualAllocEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess, IntPtr lpAddress,
            uint dwSize, uint flAllocationType,
            uint flProtect);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
