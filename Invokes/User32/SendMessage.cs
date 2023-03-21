namespace SingleDose.Invokes.User32
{
    internal class SendMessage : IInvoke
    {
        string IInvoke.Name => "SendMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern int SendMessage(
            IntPtr hWnd, uint Msg,
            IntPtr wParam, ref COPYDATASTRUCT lParam);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, ref COPYDATASTRUCT lParam)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint) , typeof(IntPtr) , Type.GetType(typeof(COPYDATASTRUCT) + ""&"")};
            Object[] args = { hWnd, Msg, wParam, lParam };
            object res = DynamicPInvokeBuilder(typeof(int), ""User32.dll"", ""SendMessage"", ref args, paramTypes);

            lParam = (COPYDATASTRUCT)args[3];
            return (int)res;
        }

        {{INVOKE}}";
    }
}
