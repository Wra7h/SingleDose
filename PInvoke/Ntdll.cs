namespace SingleDose.PInvoke
{
    internal class Ntdll
    {
        public static string NtCreateSection = @"[DllImport(""ntdll.dll"", SetLastError = true, ExactSpelling = true)]
        static extern uint NtCreateSection(
            ref IntPtr SectionHandle,
            uint DesiredAccess,
            IntPtr ObjectAttributes,
            ref uint MaximumSize,
            uint SectionPageProtection,
            uint AllocationAttributes,
            IntPtr FileHandle);

        {{PINVOKE}}";

        public static string NtMapViewOfSection = @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern uint NtMapViewOfSection(
            IntPtr SectionHandle,
            IntPtr ProcessHandle,
            ref IntPtr BaseAddress,
            UIntPtr ZeroBits,
            UIntPtr CommitSize,
            out ulong SectionOffset,
            out uint ViewSize,
            uint InheritDisposition,
            uint AllocationType,
            uint Win32Protect);

        {{PINVOKE}}";

        public static string NtTestAlert = @"[DllImport(""ntdll.dll"", SetLastError = true)]
        public static extern uint NtTestAlert();

        {{PINVOKE}}";

        public static string NtQueryInformationProcess = @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern int NtQueryInformationProcess(
            IntPtr hProcess,
            int ProcessInfoClass,
            out PROCESS_BASIC_INFORMATION pbi,
            int cb,
            out int pSize);

        {{PINVOKE}}";

        public static string RtlCreateUserThread = @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern IntPtr RtlCreateUserThread(
            IntPtr processHandle,
            IntPtr threadSecurity,
            bool createSuspended,
            Int32 stackZeroBits,
            IntPtr stackReserved,
            IntPtr stackCommit,
            IntPtr startAddress,
            IntPtr parameter,
            ref IntPtr threadHandle,
            IntPtr clientId);

        {{PINVOKE}}";
    }
}
