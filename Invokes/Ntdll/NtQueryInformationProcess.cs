using System;

namespace SingleDose.Invokes.Ntdll
{
    internal class NtQueryInformationProcess : IInvoke
    {
        string IInvoke.Name => "NtQueryInformationProcess";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern int NtQueryInformationProcess(
            IntPtr hProcess, int ProcessInfoClass,
            out PROCESS_BASIC_INFORMATION pbi,
            int cb, out int pSize);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
