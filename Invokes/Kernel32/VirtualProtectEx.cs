using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class VirtualProtectEx : IInvoke
    {
        string IInvoke.Name => "VirtualProtectEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool VirtualProtectEx(
            IntPtr hProcess, IntPtr lpAddress, int dwSize,
            uint flNewProtect, out uint lpflOldProtect);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
