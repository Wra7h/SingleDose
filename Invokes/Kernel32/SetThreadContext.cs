namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadContext : IInvoke
    {
        string IInvoke.Name => "SetThreadContext";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool SetThreadContext(IntPtr hThread, IntPtr lpContext)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { hThread, lpContext };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""SetThreadContext"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
