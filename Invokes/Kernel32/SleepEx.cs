using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SleepEx : IInvoke
    {
        string IInvoke.Name => "SleepEx";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern int SleepEx(
             UInt32 dwMilliseconds,bool bAlertable);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
