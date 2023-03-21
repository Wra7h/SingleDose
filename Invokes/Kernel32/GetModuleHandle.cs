namespace SingleDose.Invokes.Kernel32
{
    internal class GetModuleHandle : IInvoke
    {
        string IInvoke.Name => "GetModuleHandle";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr GetModuleHandle(string lpModuleName)
        {
            Type[] paramTypes = { typeof(string) };
            Object[] args = { lpModuleName };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""GetModuleHandle"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
