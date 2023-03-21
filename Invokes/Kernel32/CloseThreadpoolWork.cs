namespace SingleDose.Invokes.Kernel32
{
    internal class CloseThreadpoolWork : IInvoke
    {
        string IInvoke.Name => "CloseThreadpoolWork";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolWork(IntPtr pwk);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static void CloseThreadpoolWork(IntPtr pwk)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { pwk };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""CloseThreadpoolWork"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
