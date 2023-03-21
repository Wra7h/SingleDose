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

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect)
        {
            lpflOldProtect = 0x0;
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(int), typeof(uint), Type.GetType(typeof(uint) + ""&"") };
            Object[] args = { hProcess, lpAddress, dwSize, flNewProtect, lpflOldProtect };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""VirtualProtectEx"", ref args, paramTypes);
            lpflOldProtect = (uint)args[4];
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
