namespace SingleDose.Invokes.Setupapi
{
    internal class SetupQueueCopy : IInvoke
    {
        string IInvoke.Name => "SetupQueueCopy";

        string IInvoke.PInvoke => @"[DllImport(""Setupapi.dll"")]
        static extern bool SetupQueueCopy(
            IntPtr QueueHandle, string SourceRootPath, 
            string SourcePath, string SourceFilename, 
            string SourceDescription, string SourceTagFile, 
            string TargetDirectory, string TargetFilename, 
            uint CopyStyle);


        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool SetupQueueCopy(IntPtr QueueHandle, string SourceRootPath, 
            string SourcePath, string SourceFilename, string SourceDescription, string SourceTagFile, 
            string TargetDirectory, string TargetFilename, uint CopyStyle)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(string), typeof(string), typeof(string), 
                    typeof(string), typeof(string), typeof(string), typeof(string), typeof(uint) };
            Object[] args = { QueueHandle, SourceRootPath, SourcePath, SourceFilename, SourceDescription,
                SourceTagFile, TargetDirectory, TargetFilename, CopyStyle };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Setupapi.dll"", ""SetupQueueCopy"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
