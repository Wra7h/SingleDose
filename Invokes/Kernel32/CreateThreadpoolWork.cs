namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWork(
            IntPtr pfnwk, IntPtr pv, IntPtr pcbe);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateThreadpoolWork(IntPtr pfnwk, IntPtr pv, IntPtr pcbe)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { pfnwk, pv, pcbe };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateThreadpoolWork"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
