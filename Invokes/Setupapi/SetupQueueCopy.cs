using System;

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


        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
