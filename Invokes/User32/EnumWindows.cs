namespace SingleDose.Invokes.User32
{
    internal class EnumWindows : IInvoke
    {
        string IInvoke.Name => "EnumWindows";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        static extern bool EnumWindows(
            IntPtr lpEnumFunc, IntPtr lParam);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool EnumWindows(IntPtr lpEnumFunc, IntPtr lParam)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { lpEnumFunc, lParam };
            object res = DynamicPInvokeBuilder(typeof(bool), ""User32.dll"", ""EnumWindows"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
