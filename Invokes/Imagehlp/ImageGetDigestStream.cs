using System;

namespace SingleDose.Invokes.Imagehlp
{
    internal class ImageGetDigestStream : IInvoke
    {
        string IInvoke.Name => "ImageGetDigestStream";

        string IInvoke.PInvoke => @"[DllImport(""Imagehlp.dll"")]
        static extern bool ImageGetDigestStream(
            IntPtr FileHandle, uint DigestLevel, 
            IntPtr DigestFunction, IntPtr DigestHandle);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
