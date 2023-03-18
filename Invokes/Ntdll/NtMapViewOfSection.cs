using System;

namespace SingleDose.Invokes.Ntdll
{
    internal class NtMapViewOfSection : IInvoke
    {
        string IInvoke.Name => "NtMapViewOfSection";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern uint NtMapViewOfSection(
            IntPtr SectionHandle, IntPtr ProcessHandle,
            ref IntPtr BaseAddress, UIntPtr ZeroBits,
            UIntPtr CommitSize, out ulong SectionOffset,
            out uint ViewSize, uint InheritDisposition,
            uint AllocationType, uint Win32Protect);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
