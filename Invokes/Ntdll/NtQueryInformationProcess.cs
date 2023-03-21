namespace SingleDose.Invokes.Ntdll
{
    internal class NtQueryInformationProcess : IInvoke
    {
        string IInvoke.Name => "NtQueryInformationProcess";

        string IInvoke.PInvoke => @"[DllImport(""ntdll.dll"", SetLastError = true)]
        static extern int NtQueryInformationProcess(
            IntPtr hProcess, int ProcessInfoClass,
            out PROCESS_BASIC_INFORMATION pbi,
            int cb, out int pSize);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static int NtQueryInformationProcess(IntPtr hProcess, int ProcessInfoClass,
            out PROCESS_BASIC_INFORMATION pbi, int cb, out int pSize)
        {
            pbi = new PROCESS_BASIC_INFORMATION();
            pSize = 0;
            Type[] paramTypes = { typeof(IntPtr), typeof(int), Type.GetType(typeof(PROCESS_BASIC_INFORMATION) + ""&""), typeof(int), Type.GetType(typeof(int) + ""&"")};
            Object[] args = { hProcess, ProcessInfoClass, pbi, cb, pSize };

            object res = DynamicPInvokeBuilder(typeof(int), ""ntdll.dll"", ""NtQueryInformationProcess"", ref args, paramTypes);
            pbi = (PROCESS_BASIC_INFORMATION)args[2];
            pSize = (int)args[4];
            return (int)res;
        }

        {{INVOKE}}";
    }
}
