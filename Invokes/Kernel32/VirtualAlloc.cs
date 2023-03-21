namespace SingleDose.Invokes.Kernel32
{
    internal class VirtualAlloc : IInvoke
    {
        string IInvoke.Name => "VirtualAlloc";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"")]
        public static extern IntPtr VirtualAlloc(
            IntPtr lpAddress, uint dwSize, 
            uint flAllocationType, uint flProtect);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint), typeof(uint), typeof(uint) };
            Object[] args = { lpAddress, dwSize, flAllocationType, flProtect };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""VirtualAlloc"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
