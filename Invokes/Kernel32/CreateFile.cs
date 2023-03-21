namespace SingleDose.Invokes.Kernel32
{
    internal class CreateFile : IInvoke
    {
        string IInvoke.Name => "CreateFile";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateFile(
            string lpFileName, UInt32 dwDesiredAccess, 
            UInt32 dwShareMode, IntPtr lpSecurityAttributes, 
            UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, 
            IntPtr hTemplateFile);


        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateFile(
            string lpFileName, UInt32 dwDesiredAccess, UInt32 dwShareMode, IntPtr lpSecurityAttributes, 
            UInt32 dwCreationDisposition, UInt32 dwFlagsAndAttributes, IntPtr hTemplateFile)
        {
            Type[] paramTypes = { typeof(string), typeof(UInt32), typeof(UInt32), typeof(IntPtr), typeof(UInt32), typeof(UInt32), typeof(IntPtr) };
            Object[] args = { lpFileName, dwDesiredAccess, dwShareMode, lpSecurityAttributes, dwCreationDisposition, dwFlagsAndAttributes, hTemplateFile };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateFile"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
