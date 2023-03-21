namespace SingleDose.Invokes.Kernel32
{
    internal class GetCurrentThread : IInvoke
    {
        string IInvoke.Name => "GetCurrentThread";

        string IInvoke.PInvoke =>  @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr GetCurrentThread()
        {
            Type[] paramTypes = { };
            Object[] args = { };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""GetCurrentThread"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
