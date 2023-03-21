namespace SingleDose.Invokes.Kernel32
{
    internal class GetThreadContext : IInvoke
    {
        string IInvoke.Name => "GetThreadContext";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool GetThreadContext(IntPtr hThread, IntPtr lpContext)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { hThread, lpContext };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""GetThreadContext"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
