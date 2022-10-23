namespace SingleDose.PInvoke
{
    internal class ResUtils
    {
        public static string ClusWorkerCreate = @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerCreate(out CLUS_WORKER lpWorker,
            IntPtr lpStartAddress, IntPtr lpParameter);
        
        {{PINVOKE}}";

        public static string ClusWorkerTerminateEx = @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerTerminateEx(ref CLUS_WORKER ClusWorker,
            uint TimeoutInMilliseconds, bool WaitOnly);
        
        {{PINVOKE}}";

    }
}
