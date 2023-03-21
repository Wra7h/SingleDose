namespace SingleDose.Invokes.ResUtils
{
    internal class ClusWorkerTerminateEx : IInvoke
    {
        string IInvoke.Name => "ClusWorkerTerminateEx";

        string IInvoke.PInvoke => @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerTerminateEx(ref CLUS_WORKER ClusWorker,
            uint TimeoutInMilliseconds, bool WaitOnly);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr ClusWorkerTerminateEx(ref CLUS_WORKER ClusWorker, uint TimeoutInMilliseconds, bool WaitOnly)
        {
            Type[] paramTypes = { Type.GetType(typeof(CLUS_WORKER) + ""&""), typeof(uint), typeof(bool) };
            Object[] args = { ClusWorker, TimeoutInMilliseconds, WaitOnly };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""ResUtils.dll"", ""ClusWorkerTerminateEx"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
