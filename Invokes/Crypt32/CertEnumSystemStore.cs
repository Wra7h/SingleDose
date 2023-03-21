namespace SingleDose.Invokes.Crypt32
{
    internal class CertEnumSystemStore : IInvoke
    {
        string IInvoke.Name => "CertEnumSystemStore";

        string IInvoke.PInvoke => @"[DllImport(""Crypt32.dll"")]
        static extern bool CertEnumSystemStore(
            uint dwFlags, IntPtr pvSystemStoreLocationPara,
            IntPtr pvArg, IntPtr pfnEnum);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool CertEnumSystemStore(uint dwFlags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, IntPtr pfnEnum)
        {
            Type[] paramTypes = { typeof(uint), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { dwFlags, pvSystemStoreLocationPara, pvArg, pfnEnum };
            object res = DynamicPInvokeBuilder(typeof(bool), ""Crypt32.dll"", ""CertEnumSystemStore"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
