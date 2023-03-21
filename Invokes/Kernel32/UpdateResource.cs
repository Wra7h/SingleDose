namespace SingleDose.Invokes.Kernel32
{
    internal class UpdateResource : IInvoke
    {
        string IInvoke.Name => "UpdateResource";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool UpdateResource(
            IntPtr hUpdate, IntPtr lpType,
            IntPtr lpName, ushort wLanguage,
             byte[] lpData, uint cb);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, byte[] lpData, uint cb)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(ushort), typeof(byte[]), typeof(uint) };
            Object[] args = { hUpdate, lpType, lpName, wLanguage, lpData, cb };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""UpdateResource"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
