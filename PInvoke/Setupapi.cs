namespace SingleDose.PInvoke
{
    internal class Setupapi
    {

        public static string SetupCommitFileQueue = @"[DllImport(""Setupapi.dll"")]
        static extern bool SetupCommitFileQueue(
            IntPtr hWndOwner,
            IntPtr QueueHandle,
            IntPtr MsgHandler,
            IntPtr pContext);

        {{PINVOKE}}";


        public static string SetupOpenFileQueue = @"[DllImport(""Setupapi.dll"")]
        static extern IntPtr SetupOpenFileQueue();

        {{PINVOKE}}";

        public static string SetupQueueCopy = @"[DllImport(""Setupapi.dll"")]
        static extern bool SetupQueueCopy(
            IntPtr QueueHandle, 
            string SourceRootPath, 
            string SourcePath, 
            string SourceFilename, 
            string SourceDescription, 
            string SourceTagFile, 
            string TargetDirectory, 
            string TargetFilename, 
            uint CopyStyle);


        {{PINVOKE}}";
    }
}
