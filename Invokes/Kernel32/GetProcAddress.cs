namespace SingleDose.Invokes.Kernel32
{
    internal class GetProcAddress : IInvoke
    {
        string IInvoke.Name => "GetProcAddress";

        string IInvoke.PInvoke => @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr GetProcAddress(IntPtr hModule, string procName)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(string) };
            Object[] args = { hModule, procName };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""GetProcAddress"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
