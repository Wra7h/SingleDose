namespace SingleDose.PInvoke
{
    internal class Kernel32
    {
        public static string BeginUpdateResource = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr BeginUpdateResource(string pFileName, bool bDeleteExistingResources);

        {{PINVOKE}}";

        public static string CloseHandle = @"[DllImport(""kernel32.dll"")]
        static extern bool CloseHandle(IntPtr hObject);

        {{PINVOKE}}";
        
        public static string CloseThreadpoolTimer = @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolTimer(
            IntPtr pti);
        
        {{PINVOKE}}";

        public static string CloseThreadpoolWork = @"[DllImport(""kernel32.dll"")]
        static extern void CloseThreadpoolWork(
            IntPtr pwk);
        
        {{PINVOKE}}";

        public static string ConvertThreadToFiber = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr ConvertThreadToFiber(IntPtr lpParameter);

        {{PINVOKE}}";
        
        public static string CreateEvent = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes,
            bool bManualReset,
            bool bInitialState,
            string lpName);
            
        {{PINVOKE}}";

        public static string CreateFiber = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFiber(
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter);

        {{PINVOKE}}";

        public static string CreateFile = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFile(
            string lpFileName, 
            UInt32 dwDesiredAccess, 
            UInt32 dwShareMode, 
            IntPtr lpSecurityAttributes, 
            UInt32 dwCreationDisposition, 
            UInt32 dwFlagsAndAttributes, 
            IntPtr hTemplateFile);


        {{PINVOKE}}";

        public static string CreateProcess = @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        static extern bool CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandles,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartuprocInfo,
            out PROCESS_INFORMATION lpProcessInformation);

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

        public static string CreateThread = @"[DllImport(""kernel32"", CharSet = CharSet.Ansi)]
        public static extern IntPtr CreateThread(
            IntPtr lpThreadAttributes,
            uint dwStackSize,
            IntPtr lpStartAddress,
            IntPtr lpParameter,
            uint dwCreationFlags,
            IntPtr lpThreadId);

        {{PINVOKE}}";

        public static string CreateThreadpoolTimer = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolTimer(
            IntPtr pfnti,
            IntPtr pv,
            IntPtr pcbe);

        {{PINVOKE}}";

        public static string CreateThreadpoolWait = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWait(
            IntPtr pfnwa,
            IntPtr pv,
            IntPtr pcbe);

        {{PINVOKE}}";

        public static string CreateThreadpoolWork = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateThreadpoolWork(
            IntPtr pfnwk,
            IntPtr pv,
            IntPtr pcbe);

        {{PINVOKE}}";

        public static string CreateWaitableTimer = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateWaitableTimer(
            IntPtr lpTimerAttributes,
            bool bManualReset,
            string lpTimerName);

        {{PINVOKE}}";

        public static string EndUpdateResource = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool EndUpdateResource(IntPtr hUpdate, bool fDiscard);

        {{PINVOKE}}";

        public static string EnumDateFormatsEx = @"[DllImport(""kernel32.dll"")]
        static extern bool EnumDateFormatsEx(IntPtr lpDateFmtEnumProcEx, uint Locale, uint dwFlags);

        {{PINVOKE}}";

        public static string FlsAlloc = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern uint FlsAlloc(IntPtr callback);

        {{PINVOKE}}";

        public static string FlsSetValue = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool FlsSetValue(uint dwFlsIndex, string lpFlsData);

        {{PINVOKE}}";

        public static string GetCurrentThread = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern IntPtr GetCurrentThread();

        {{PINVOKE}}";

        public static string GetModuleHandle = @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        {{PINVOKE}}";

        public static string GetProcAddress = @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        {{PINVOKE}}";

        public static string GetThreadContext = @"[DllImport(""kernel32"", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern bool GetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{PINVOKE}}";

        public static string OpenThread = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern IntPtr OpenThread(int dwDesiredAccess, bool bInheritHandle, int dwThreadId);

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


        public static string SetThreadContext = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool SetThreadContext(IntPtr hThread, IntPtr lpContext);

        {{PINVOKE}}";

        public static string SetThreadpoolTimer = @"[DllImport(""kernel32.dll"")]
        static extern void SetThreadpoolTimer(
            IntPtr pti,
            ref FILETIME pv,
            uint msPeriod,
            uint msWindowLength);
            
        {{PINVOKE}}";

        public static string SetThreadpoolWait = @"[DllImport(""kernel32.dll"")]
        static extern IntPtr SetThreadpoolWait(
            IntPtr pwa,
            IntPtr h,
            IntPtr pftTimeout);

        {{PINVOKE}}";

        public static string SleepEx = @"[DllImport(""kernel32.dll"")]
        static extern int SleepEx(
             UInt32 dwMilliseconds,
             bool bAlertable);

        {{PINVOKE}}";

        public static string SetWaitableTimer = @"[DllImport(""kernel32.dll"")]
        static extern bool SetWaitableTimer(IntPtr hTimer,
            ref LARGE_INTEGER pDueTime,
            int lPeriod,
            IntPtr pfnCompletionRoutine,
            IntPtr lpArgToCompletionRoutine,
            bool fResume);

        {{PINVOKE}}";

        public static string SubmitThreadpoolWork = @"[DllImport(""kernel32.dll"")]
        static extern void SubmitThreadpoolWork(
            IntPtr pwkl);

        {{PINVOKE}}";

        public static string SwitchToFiber = @"[DllImport(""kernel32.dll"")]
        extern static IntPtr SwitchToFiber(IntPtr lpFiber);

        {{PINVOKE}}";

        public static string SuspendThread = @"[DllImport(""kernel32.dll"")]
        static extern uint SuspendThread(IntPtr hThread);

        {{PINVOKE}}";

        public static string UpdateResource = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        public static extern bool UpdateResource(IntPtr hUpdate, IntPtr lpType, IntPtr lpName, ushort wLanguage, byte[] lpData, uint cb);

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
            IntPtr hProcess, IntPtr lpAddress, int dwSize,
            uint flNewProtect, out uint lpflOldProtect);

        {{PINVOKE}}";

        public static string WaitForSingleObject = @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern UInt32 WaitForSingleObject(
            IntPtr hHandle,
            UInt32 dwMilliseconds);

        {{PINVOKE}}";

        public static string WaitForThreadpoolTimerCallbacks = @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolTimerCallbacks(
            IntPtr pti,
            bool fCancelPendingCallbacks);

        {{PINVOKE}}";

        public static string WaitForThreadpoolWorkCallbacks = @"[DllImport(""kernel32.dll"")]
        static extern void WaitForThreadpoolWorkCallbacks(
            IntPtr pwk,
            bool fCancelPendingCallbacks);

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
