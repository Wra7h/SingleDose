namespace SingleDose.Invokes.User32
{
    internal class GetTopWindow : IInvoke
    {
        string IInvoke.Name => "GetTopWindow";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern IntPtr GetTopWindow(IntPtr hWnd);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr GetTopWindow(IntPtr hWnd)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { hWnd };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""User32.dll"", ""GetTopWindow"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
