using System;

namespace SingleDose.Invokes.Kernel32
{
    internal class CreateProcess : IInvoke
    {
        string IInvoke.Name => "CreateProcess";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        static extern bool CreateProcess(
            string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags,
            IntPtr lpEnvironment, string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartuprocInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
