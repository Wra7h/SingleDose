using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Advapi32
{
    internal class PerfStopProvider : IInvoke
    {
        string IInvoke.Name => "PerfStopProvider";

        string IInvoke.PInvoke => @"[DllImport(""advapi32.dll"")]
        static extern ulong PerfStopProvider(IntPtr hProvider);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static ulong PerfStopProvider(IntPtr hProvider)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { hProvider };
            object res = DynamicPInvokeBuilder(typeof(ulong), ""advapi32.dll"", ""PerfStopProvider"", ref args, paramTypes);
            return (ulong)res;
        }

        {{INVOKE}}";
    }
}
