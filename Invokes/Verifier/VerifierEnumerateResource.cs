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

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static ulong VerifierEnumerateResource(IntPtr hProcess, ulong flags, 
            ulong ResourceType, IntPtr ResourceCallback, IntPtr EnumerationContext)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(ulong) , typeof(ulong) , typeof(IntPtr) , typeof(IntPtr)};
            Object[] args = { hProcess, flags, ResourceType, ResourceCallback, EnumerationContext };
            object res = DynamicPInvokeBuilder(typeof(ulong), ""Verifier.dll"", ""VerifierEnumerateResource"", ref args, paramTypes);
            return (ulong)res;
        }

        {{INVOKE}}";
    }
}
