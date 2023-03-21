namespace SingleDose.Invokes.Kernel32
{
    internal class EndUpdateResource : IInvoke
    {
        string IInvoke.Name => "EndUpdateResource";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool EndUpdateResource(IntPtr hUpdate, bool fDiscard)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(bool) };
            Object[] args = { hUpdate, fDiscard };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""EndUpdateResource"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
