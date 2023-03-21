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

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr CreateThread(
            IntPtr lpThreadAttributes, uint dwStackSize,
            IntPtr lpStartAddress, IntPtr lpParameter,
            uint dwCreationFlags, IntPtr lpThreadId)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(uint), typeof(IntPtr) };
            Object[] args = { lpThreadAttributes, dwStackSize, lpStartAddress, lpParameter, dwCreationFlags, lpThreadId };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateThread"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
