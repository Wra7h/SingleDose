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

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(uint), typeof(uint) };
            Object[] args = { hProcess, lpAddress, dwSize, flAllocationType, flProtect };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""VirtualAllocEx"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
