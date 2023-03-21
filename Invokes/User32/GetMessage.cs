namespace SingleDose.Invokes.User32
{
    internal class GetMessage : IInvoke
    {
        string IInvoke.Name => "GetMessage";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"")]
        static extern int GetMessage(
            out MSG lpMsg, IntPtr hWnd, 
            uint wMsgFilterMin, uint wMsgFilterMax);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax)
        {
            lpMsg = new MSG();
            Type[] paramTypes = { Type.GetType(typeof(MSG) + ""&""), typeof(IntPtr), typeof(uint), typeof(uint) };
            Object[] args = { lpMsg, hWnd, wMsgFilterMin, wMsgFilterMax };
            object res = DynamicPInvokeBuilder(typeof(int), ""User32.dll"", ""GetMessage"", ref args, paramTypes);
            lpMsg = (MSG)args[0];

            return (int)res;
        }

        {{INVOKE}}";
    }
}
