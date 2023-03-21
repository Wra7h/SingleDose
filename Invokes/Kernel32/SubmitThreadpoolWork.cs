namespace SingleDose.Invokes.Kernel32
{
    internal class SubmitThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "SubmitThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void SubmitThreadpoolWork(IntPtr pwkl);

        {{INVOKE}}";
        string IInvoke.DInvoke => @"public static void SubmitThreadpoolWork(IntPtr pwkl)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { pwkl };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""SubmitThreadpoolWork"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
