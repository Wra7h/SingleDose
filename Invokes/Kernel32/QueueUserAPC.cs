namespace SingleDose.Invokes.Kernel32
{
    internal class QueueUserAPC : IInvoke
    {
        string IInvoke.Name => "QueueUserAPC";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { pfnAPC, hThread, dwData };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""QueueUserAPC"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
