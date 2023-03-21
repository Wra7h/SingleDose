namespace SingleDose.Invokes.Setupapi
{
    internal class SetupCommitFileQueue : IInvoke
    {
        string IInvoke.Name => "SetupCommitFileQueue";

        string IInvoke.PInvoke => @"[DllImport(""Setupapi.dll"")]
        static extern bool SetupCommitFileQueue(
            IntPtr hWndOwner, IntPtr QueueHandle,
            IntPtr MsgHandler, IntPtr pContext);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool SetupCommitFileQueue(IntPtr hWndOwner, IntPtr QueueHandle, IntPtr MsgHandler, IntPtr pContext)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(IntPtr), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { hWndOwner, QueueHandle, MsgHandler, pContext };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Setupapi.dll"", ""SetupCommitFileQueue"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
