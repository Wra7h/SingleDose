namespace SingleDose.Invokes.Kernel32
{
    internal class SwitchToFiber : IInvoke
    {
        string IInvoke.Name => "SwitchToFiber";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        extern static IntPtr SwitchToFiber(IntPtr lpFiber);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr SwitchToFiber(IntPtr lpFiber)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { lpFiber };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""SwitchToFiber"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
