namespace SingleDose.Invokes.Kernel32
{
    internal class CloseThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "CloseThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolTimer(IntPtr pti);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static void CloseThreadpoolTimer(IntPtr pti)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { pti };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""CloseThreadpoolTimer"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
