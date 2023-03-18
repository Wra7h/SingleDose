using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class EndUpdateResource : IInvoke
    {
        string IInvoke.Name => "EndUpdateResource";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
