using System;

namespace SingleDose.Invokes.Ntdll
{
    internal class NtCreateSection : IInvoke
    {
        string IInvoke.Name => "NtCreateSection";
        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true, ExactSpelling = true)]
        static extern uint NtCreateSection(
            ref IntPtr SectionHandle, uint DesiredAccess, IntPtr ObjectAttributes,
            ref uint MaximumSize, uint SectionPageProtection, uint AllocationAttributes,
            IntPtr FileHandle);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
