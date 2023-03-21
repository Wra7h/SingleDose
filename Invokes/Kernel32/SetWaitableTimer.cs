namespace SingleDose.Invokes.Kernel32
{
    internal class SetWaitableTimer : IInvoke
    {
        string IInvoke.Name => "SetWaitableTimer";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool SetWaitableTimer(IntPtr hTimer,
            ref LARGE_INTEGER pDueTime, int lPeriod,
            IntPtr pfnCompletionRoutine, 
            IntPtr lpArgToCompletionRoutine,
            bool fResume);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool SetWaitableTimer(IntPtr hTimer, ref LARGE_INTEGER pDueTime, int lPeriod,
            IntPtr pfnCompletionRoutine, IntPtr lpArgToCompletionRoutine, bool fResume)
        {
            Type[] paramTypes = { typeof(IntPtr), Type.GetType(typeof(LARGE_INTEGER)+""&""), typeof(int), typeof(IntPtr), typeof(IntPtr), typeof(bool) };
            Object[] args = { hTimer, pDueTime, lPeriod, pfnCompletionRoutine, lpArgToCompletionRoutine, fResume };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""SetWaitableTimer"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
