using System;

namespace SingleDose.Invokes.Setupapi
{
    internal class SetupCommitFileQueue : IInvoke
    {
        string IInvoke.Name => "SetupCommitFileQueue";

        string IInvoke.PInvoke => @"[DllImport(""Setupapi.dll"")]
        static extern bool SetupCommitFileQueue(
            IntPtr hWndOwner, IntPtr QueueHandle,
            IntPtr MsgHandler, IntPtr pContext);

        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
