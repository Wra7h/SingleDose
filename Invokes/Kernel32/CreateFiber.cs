namespace SingleDose.Invokes.Kernel32
{
    internal class CreateFiber : IInvoke
    {
        string IInvoke.Name => "CreateFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFiber(
            uint dwStackSize, IntPtr lpStartAddress,
            IntPtr lpParameter);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateFiber( uint dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter)
        {
            Type[] paramTypes = { typeof(uint), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { dwStackSize, lpStartAddress, lpParameter };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateFiber"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
