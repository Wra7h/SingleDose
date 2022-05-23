namespace SingleDose.PInvoke
{
    internal class Kernel32
    {
        public static string ConvertThreadToFiber = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr ConvertThreadToFiber(IntPtr lpParameter);

        {{PINVOKE}}";

        public static string CreateProcess = @"[DllImport(""kernel32.dll"", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            ref SecurityAttributes lpProcessAttributes,
            ref SecurityAttributes lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartuprocInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        {{PINVOKE}}";

        public static string CreateFiber = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFiber(
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter);

        {{PINVOKE}}";

        public static string CreateRemoteThread = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateRemoteThread(
            IntPtr hProcess,
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        {{PINVOKE}}";

        public static string EnumDateFormatsEx = @"[DllImport(""kernel32.dll"")]
        static extern bool EnumDateFormatsEx(IntPtr lpDateFmtEnumProcEx, uint Locale, uint dwFlags);

        {{PINVOKE}}";

        public static string GetModuleHandle = @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        {{PINVOKE}}";

        public static string GetProcAddress = @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        {{PINVOKE}}";

        public static string OpenThread = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

        {{PINVOKE}}";

        public static string QueueUserAPC = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr QueueUserAPC(IntPtr pfnAPC, IntPtr hThread, IntPtr dwData);

        {{PINVOKE}}";

        public static string ReadProcessMemory = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool ReadProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            Int32 nSize,
            out IntPtr lpNumberOfBytesRead);

        {{PINVOKE}}";

        public static string ResumeThread = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint ResumeThread(IntPtr hThread);

        {{PINVOKE}}";

        public static string SwitchToFiber = @"[DllImport(""kernel32.dll"")]
        extern static IntPtr SwitchToFiber(IntPtr lpFiber);

        {{PINVOKE}}";

        public static string VirtualAlloc = @"[DllImport(""kernel32"")]
        public static extern IntPtr VirtualAlloc(
            IntPtr lpAddress, 
            uint dwSize, 
            uint flAllocationType,
            uint flProtect);

        {{PINVOKE}}";

        public static string VirtualAllocEx = @"[DllImport(""kernel32.dll"", SetLastError = true, ExactSpelling = true)]
        static extern IntPtr VirtualAllocEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            uint dwSize,
            uint flAllocationType,
            uint flProtect);

        {{PINVOKE}}";

        public static string VirtualProtectEx = @"[DllImport(""kernel32.dll"")]
        static extern bool VirtualProtectEx(
            IntPtr hProcess,
            IntPtr lpAddress,
            int dwSize,
            uint flNewProtect,
            out uint lpflOldProtect);

        {{PINVOKE}}";

        public static string WriteProcessMemory_ByteArray = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool WriteProcessMemory(
            IntPtr hProcess,
            IntPtr lpBaseAddress,
            byte[] lpBuffer,
            uint nSize,
            out IntPtr lpNumberOfBytesWritten);

        {{PINVOKE}}";

        public static string WriteProcessMemory_IntPtr = @"[DllImport(""kernel32.dll"")]
        static extern bool WriteProcessMemory(
             IntPtr hProcess,
             IntPtr lpBaseAddress,
             IntPtr lpBuffer,
             Int32 nSize,
             out IntPtr lpNumberOfBytesWritten);

        {{PINVOKE}}";
    }
}
