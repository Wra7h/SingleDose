namespace SingleDose.PInvoke
{
    internal class Verifier
    {
        public static string VerifierEnumerateResource = @"[DllImport(""Verifier.dll"")]
        static extern ulong VerifierEnumerateResource(
            IntPtr hProcess,
            ulong flags,
            ulong ResourceType,
            IntPtr ResourceCallback,
            IntPtr EnumerationContext);

        {{PINVOKE}}";
    }
}
