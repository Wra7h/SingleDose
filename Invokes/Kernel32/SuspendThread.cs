using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class SuspendThread : IInvoke
    {
        string IInvoke.Name => "SuspendThread";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern uint SuspendThread(IntPtr hThread);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
