namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadpoolWait : IInvoke
    {
        string IInvoke.Name => "SetThreadpoolWait";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr SetThreadpoolWait(
            IntPtr pwa, IntPtr h, IntPtr pftTimeout);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr SetThreadpoolWait(IntPtr pwa, IntPtr h, IntPtr pftTimeout)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { pwa, h, pftTimeout };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""SetThreadpoolWait"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
