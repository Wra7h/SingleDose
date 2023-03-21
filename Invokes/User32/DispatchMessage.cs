namespace SingleDose.Invokes.User32
{
    internal class DispatchMessage : IInvoke
    {
        string IInvoke.Name => "DispatchMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern IntPtr DispatchMessage([In] ref MSG lpmsg);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr DispatchMessage([In] ref MSG lpmsg)
        {
            Type[] paramTypes = { typeof(MSG) };
            Object[] args = { lpmsg };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""User32.dll"", ""DispatchMessage"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
