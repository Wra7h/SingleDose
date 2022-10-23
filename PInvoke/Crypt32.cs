namespace SingleDose.PInvoke
{
    internal class Crypt32
    {
        public static string CertEnumSystemStore = @"[DllImport(""Crypt32.dll"")]
        static extern bool CertEnumSystemStore(
            uint dwFlags,
            IntPtr pvSystemStoreLocationPara,
            IntPtr pvArg,
            IntPtr pfnEnum);

        {{PINVOKE}}";
    }
}
