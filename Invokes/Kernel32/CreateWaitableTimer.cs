namespace SingleDose.Invokes.Kernel32
{
    internal class CreateWaitableTimer : IInvoke
    {
        string IInvoke.Name => "CreateWaitableTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateWaitableTimer(
            IntPtr lpTimerAttributes, bool bManualReset,
            string lpTimerName);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(bool), typeof(string) };
            Object[] args = { lpTimerAttributes, bManualReset, lpTimerName };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateWaitableTimer"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
