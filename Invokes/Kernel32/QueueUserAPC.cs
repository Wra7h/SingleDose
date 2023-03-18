using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class QueueUserAPC : IInvoke
    {
        string IInvoke.Name => "QueueUserAPC";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
