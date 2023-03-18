using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class FlsSetValue : IInvoke
    {
        string IInvoke.Name => "FlsSetValue";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool FlsSetValue(uint dwFlsIndex, string lpFlsData);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
