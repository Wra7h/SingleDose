namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForThreadpoolWorkCallbacks : IInvoke
    {
        string IInvoke.Name => "WaitForThreadpoolWorkCallbacks";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolWorkCallbacks(
            IntPtr pwk, bool fCancelPendingCallbacks);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static void WaitForThreadpoolWorkCallbacks(IntPtr pwk, bool fCancelPendingCallbacks)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(bool) };
            Object[] args = { pwk, fCancelPendingCallbacks };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""WaitForThreadpoolWorkCallbacks"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
