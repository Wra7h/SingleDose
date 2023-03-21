namespace SingleDose.Invokes.User32
{
    internal class EnumChildWindows : IInvoke
    {
        string IInvoke.Name => "EnumChildWindows";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        public static extern bool EnumChildWindows(
            IntPtr hWndParent, IntPtr lpEnumFunc,
            IntPtr lParam);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool EnumChildWindows(IntPtr hWndParent, IntPtr lpEnumFunc, IntPtr lParam)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { hWndParent, lpEnumFunc, lParam };
            object res = DynamicPInvokeBuilder(typeof(bool), ""User32.dll"", ""EnumChildWindows"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
