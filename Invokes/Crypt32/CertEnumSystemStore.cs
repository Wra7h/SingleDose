using System;

namespace SingleDose.Invokes.Crypt32
{
    internal class CertEnumSystemStore : IInvoke
    {
        string IInvoke.Name => "CertEnumSystemStore";

        string IInvoke.PInvoke => @"[DllImport(""Crypt32.dll"")]
        static extern bool CertEnumSystemStore(
            uint dwFlags, IntPtr pvSystemStoreLocationPara,
            IntPtr pvArg, IntPtr pfnEnum);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
