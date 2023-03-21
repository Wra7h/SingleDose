namespace SingleDose.Invokes.Kernel32
{
    internal class ConvertThreadToFiber : IInvoke
    {
        string IInvoke.Name => "ConvertThreadToFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr ConvertThreadToFiber(IntPtr lpParameter);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr ConvertThreadToFiber(IntPtr lpParameter)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { lpParameter };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""ConvertThreadToFiber"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
