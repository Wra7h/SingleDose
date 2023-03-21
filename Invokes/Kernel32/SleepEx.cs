namespace SingleDose.Invokes.Kernel32
{
    internal class SleepEx : IInvoke
    {
        string IInvoke.Name => "SleepEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern int SleepEx( UInt32 dwMilliseconds, bool bAlertable);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static int SleepEx(UInt32 dwMilliseconds, bool bAlertable)
        {
            Type[] paramTypes = { typeof(UInt32), typeof(bool) };
            Object[] args = { dwMilliseconds, bAlertable };

            object res = DynamicPInvokeBuilder(typeof(int), ""Kernel32.dll"", ""SleepEx"", ref args, paramTypes);
            return (int)res;
        }

        {{INVOKE}}";
    }
}
