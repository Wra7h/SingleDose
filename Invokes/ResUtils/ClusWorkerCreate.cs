using System;

namespace SingleDose.Invokes.ResUtils
{
    internal class ClusWorkerCreate : IInvoke
    {
        string IInvoke.Name => "ClusWorkerCreate";

        string IInvoke.PInvoke => @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerCreate(out CLUS_WORKER lpWorker,
            IntPtr lpStartAddress, IntPtr lpParameter);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
