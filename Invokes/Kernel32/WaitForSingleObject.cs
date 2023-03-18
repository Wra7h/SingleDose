using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class WaitForSingleObject : IInvoke
    {
        string IInvoke.Name => "WaitForSingleObject";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(
            IntPtr hHandle, UInt32 dwMilliseconds);

        {{PINVOKE}}";
        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
