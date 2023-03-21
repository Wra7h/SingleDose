namespace SingleDose.Invokes.User32
{
    internal class SetTimer : IInvoke
    {
        string IInvoke.Name => "SetTimer";

        string IInvoke.PInvoke => @"[DllImport(""user32.dll"", ExactSpelling = true)]
        static extern IntPtr SetTimer(
            IntPtr hWnd, IntPtr nIDEvent, 
            uint uElapse, IntPtr lpTimerFunc);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr SetTimer(IntPtr hWnd, IntPtr nIDEvent, uint uElapse, IntPtr lpTimerFunc)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr) , typeof(uint) , typeof(IntPtr)};
            Object[] args = { hWnd, nIDEvent, uElapse, lpTimerFunc };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""User32.dll"", ""SetTimer"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
