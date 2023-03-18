using System;

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

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
