using System;

namespace SingleDose.Invokes.ResUtils
{
    internal class ClusWorkerTerminateEx : IInvoke
    {
        string IInvoke.Name => "ClusWorkerTerminateEx";

        string IInvoke.PInvoke => @"[DllImport(""ResUtils.dll"")]
        static extern IntPtr ClusWorkerTerminateEx(ref CLUS_WORKER ClusWorker,
            uint TimeoutInMilliseconds, bool WaitOnly);
        
        {{PINVOKE}}";

        string IInvoke.DInvoke => throw new NotImplementedException();
    }
}
