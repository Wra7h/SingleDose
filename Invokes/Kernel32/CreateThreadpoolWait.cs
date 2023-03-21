namespace SingleDose.Invokes.Kernel32
{
    internal class CreateThreadpoolWait : IInvoke
    {
        string IInvoke.Name => "CreateThreadpoolWait";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWait(
            IntPtr pfnwa, IntPtr pv, IntPtr pcbe);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateThreadpoolWait(IntPtr pfnwa, IntPtr pv, IntPtr pcbe)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { pfnwa, pv, pcbe };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateThreadpoolWait"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
