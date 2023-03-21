namespace SingleDose.Invokes.Kernel32
{
    internal class FlsSetValue : IInvoke
    {
        string IInvoke.Name => "FlsSetValue";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", SetLastError = true)]
        static extern bool FlsSetValue(uint dwFlsIndex, string lpFlsData);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool FlsSetValue(uint dwFlsIndex, string lpFlsData)
        {
            Type[] paramTypes = { typeof(uint), typeof(string) };
            Object[] args = { dwFlsIndex, lpFlsData };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""FlsSetValue"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
