using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Advapi32
{
    internal class PerfStartProviderEx : IInvoke
    {
        string IInvoke.Name => "PerfStartProviderEx";

        string IInvoke.PInvoke => @"[DllImport(""advapi32.dll"")]
        static extern ulong PerfStartProviderEx(
             ref Guid ProviderGuid, ref PERF_PROVIDER_CONTEXT ProviderContext,
             out IntPtr hProvider);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static ulong PerfStartProviderEx(ref Guid ProviderGuid, ref PERF_PROVIDER_CONTEXT ProviderContext,
             out IntPtr hProvider)
        {
            hProvider = IntPtr.Zero;
            Type[] paramTypes = { Type.GetType(typeof(Guid) + ""&""), Type.GetType(typeof(PERF_PROVIDER_CONTEXT) + ""&""), Type.GetType(typeof(IntPtr) + ""&"")};
            Object[] args = { ProviderGuid, ProviderContext, hProvider };
            object res = DynamicPInvokeBuilder(typeof(ulong), ""advapi32.dll"", ""PerfStartProviderEx"", ref args, paramTypes);
            ProviderGuid = (Guid) args[0];
            ProviderContext = (PERF_PROVIDER_CONTEXT) args[1];
            hProvider = (IntPtr) args[2];
            return (ulong)res;
        }

        {{INVOKE}}";
    }
}
