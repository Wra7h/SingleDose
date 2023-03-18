using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class GetCurrentThread : IInvoke
    {
        string IInvoke.Name => "GetCurrentThread";

        string IInvoke.PInvoke =>  @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
