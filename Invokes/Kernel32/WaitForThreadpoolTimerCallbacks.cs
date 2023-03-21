namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForThreadpoolTimerCallbacks : IInvoke
    {
        string IInvoke.Name => "WaitForThreadpoolTimerCallbacks";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolTimerCallbacks(IntPtr pti, bool fCancelPendingCallbacks);

        {{INVOKE}}";
        string IInvoke.DInvoke => @"public static void WaitForThreadpoolTimerCallbacks(IntPtr pti, bool fCancelPendingCallbacks)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(bool) };
            Object[] args = { pti, fCancelPendingCallbacks };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""WaitForThreadpoolTimerCallbacks"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
