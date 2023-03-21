namespace SingleDose.Invokes.Imagehlp
{
    internal class ImageGetDigestStream : IInvoke
    {
        string IInvoke.Name => "ImageGetDigestStream";

        string IInvoke.PInvoke => @"[DllImport(""Imagehlp.dll"")]
        static extern bool ImageGetDigestStream(
            IntPtr FileHandle, uint DigestLevel, 
            IntPtr DigestFunction, IntPtr DigestHandle);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool ImageGetDigestStream(IntPtr FileHandle, uint DigestLevel,
            IntPtr DigestFunction, IntPtr DigestHandle)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(uint), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { FileHandle, DigestLevel, DigestFunction, DigestHandle };
            object res = DynamicPInvokeBuilder(typeof(bool), ""Imagehlp.dll"", ""ImageGetDigestStream"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
