using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CloseHandle : IInvoke
    {
        string IInvoke.Name => "CloseHandle";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool CloseHandle(IntPtr hObject);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
