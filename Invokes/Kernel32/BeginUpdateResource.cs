namespace SingleDose.Invokes.Kernel32
{
    internal class BeginUpdateResource : IInvoke
    {
        string IInvoke.Name => "BeginUpdateResource";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources)
        {
            Type[] paramTypes = { typeof(string), typeof(bool) };
            Object[] args = { pFileName, bDeleteExistingResources };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""BeginUpdateResource"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
