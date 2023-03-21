namespace SingleDose.Invokes.Kernel32
{
    internal class SetThreadpoolTimer : IInvoke
    {
        string IInvoke.Name => "SetThreadpoolTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern void SetThreadpoolTimer(
            IntPtr pti, ref FILETIME pv,
            uint msPeriod, uint msWindowLength);
            
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static void SetThreadpoolTimer(IntPtr pti, ref FILETIME pv, uint msPeriod, uint msWindowLength)
        {
            Type[] paramTypes = { typeof(IntPtr), Type.GetType(typeof(FILETIME) + ""&""), typeof(uint), typeof(uint) };
            Object[] args = { pti, pv, msPeriod, msWindowLength };

            object res = DynamicPInvokeBuilder(typeof(void), ""Kernel32.dll"", ""SetThreadpoolTimer"", ref args, paramTypes);
            return;
        }

        {{INVOKE}}";
    }
}
