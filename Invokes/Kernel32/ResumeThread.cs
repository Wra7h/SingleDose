using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class ResumeThread : IInvoke
    {
        string IInvoke.Name => "ResumeThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
