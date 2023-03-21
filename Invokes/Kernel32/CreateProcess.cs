namespace SingleDose.Invokes.Kernel32
{
    internal class CreateProcess : IInvoke
    {
        string IInvoke.Name => "CreateProcess";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"", CharSet = CharSet.Auto)]
        static extern bool CreateProcess(
            string lpApplicationName, string lpCommandLine,
            IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags,
            IntPtr lpEnvironment, string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartuprocInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool CreateProcess(
            string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
            bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory,
            [In] ref STARTUPINFO lpStartuprocInfo, out PROCESS_INFORMATION lpProcessInformation)
        {
            lpProcessInformation = new PROCESS_INFORMATION();
            Type[] paramTypes = { typeof(string), typeof(string), typeof(IntPtr), typeof(IntPtr), typeof(bool), typeof(uint), typeof(IntPtr), typeof(string), Type.GetType(typeof(STARTUPINFO) + ""&""), Type.GetType(typeof(PROCESS_INFORMATION) + ""&"") };
            Object[] args = { lpApplicationName, lpCommandLine, lpProcessAttributes, lpThreadAttributes, bInheritHandles, dwCreationFlags, lpEnvironment, lpCurrentDirectory, lpStartuprocInfo, lpProcessInformation };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""CreateProcess"", ref args, paramTypes);
            lpProcessInformation = (PROCESS_INFORMATION)args[9];
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
