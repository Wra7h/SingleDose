using System;

namespace SingleDose.Invokes.Verifier
{
    internal class VerifierEnumerateResource : IInvoke
    {
        string IInvoke.Name => "VerifierEnumerateResource";

        string IInvoke.PInvoke => @"[DllImport(""Verifier.dll"")]
        static extern ulong VerifierEnumerateResource(
            IntPtr hProcess, ulong flags,
            ulong ResourceType, IntPtr ResourceCallback,
            IntPtr EnumerationContext);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
