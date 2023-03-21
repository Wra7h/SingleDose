namespace SingleDose.Invokes.User32
{
    internal class EnumDesktops : IInvoke
    {
        string IInvoke.Name => "EnumDesktops";

        string IInvoke.PInvoke => @"[DllImport(""User32.dll"")]
        static extern bool EnumDesktops(
            IntPtr hwinsta, IntPtr lpEnumFunc,
            IntPtr lParam);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool EnumDesktops(IntPtr hwinsta, IntPtr lpEnumFunc, IntPtr lParam)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { hwinsta, lpEnumFunc, lParam };
            object res = DynamicPInvokeBuilder(typeof(bool), ""User32.dll"", ""EnumDesktops"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
