namespace SingleDose.PInvoke
{
    internal class Imagehlp
    {
        public static string ImageGetDigestStream = @"[DllImport(""Imagehlp.dll"")]
        static extern bool ImageGetDigestStream(
            IntPtr FileHandle, 
            uint DigestLevel, 
            IntPtr DigestFunction, 
            IntPtr DigestHandle);

        {{PINVOKE}}";
    }
}
