namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolTimer(
            IntPtr pfnti, IntPtr pv, IntPtr pcbe);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateThreadpoolTimer(IntPtr pfnti, IntPtr pv, IntPtr pcbe)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { pfnti, pv, pcbe };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateThreadpoolTimer"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
