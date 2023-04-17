using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Crypt32
{
    internal class CertCreateContext : IInvoke
    {
        string IInvoke.Name => "CertCreateContext";

        string IInvoke.PInvoke => @"[DllImport(""Crypt32.dll"")]
        static extern IntPtr CertCreateContext(
            uint dwContextType, uint dwEncodingType, IntPtr pbEncoded,
            uint cbEncoded, uint dwFlags, CERT_CREATE_CONTEXT_PARA pCreatePara);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr CertCreateContext(uint dwContextType, uint dwEncodingType, IntPtr pbEncoded,
            uint cbEncoded, uint dwFlags, CERT_CREATE_CONTEXT_PARA pCreatePara)
        {
            Type[] paramTypes = { typeof(uint), typeof(uint), typeof(IntPtr), typeof(uint), typeof(uint), typeof(CERT_CREATE_CONTEXT_PARA) };
            Object[] args = { dwContextType, dwEncodingType, pbEncoded, cbEncoded, dwFlags, pCreatePara };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Crypt32.dll"", ""CertCreateContext"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
