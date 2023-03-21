namespace SingleDose.Invokes.ResUtils
{
    internal class ClusWorkerCreate : IInvoke
    {
        string IInvoke.Name => "ClusWorkerCreate";

        string IInvoke.PInvoke => @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerCreate(out CLUS_WORKER lpWorker,
            IntPtr lpStartAddress, IntPtr lpParameter);
        
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr ClusWorkerCreate(out CLUS_WORKER lpWorker, IntPtr lpStartAddress, IntPtr lpParameter)
        {
            lpWorker = new CLUS_WORKER();
            Type[] paramTypes = { Type.GetType(typeof(CLUS_WORKER) + ""&""), typeof(IntPtr), typeof(IntPtr) };
            Object[] args = { lpWorker, lpStartAddress, lpParameter };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""ResUtils.dll"", ""ClusWorkerCreate"", ref args, paramTypes);
            lpWorker = (CLUS_WORKER)args[0];
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
