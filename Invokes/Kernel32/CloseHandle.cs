namespace SingleDose.Invokes.Kernel32
{
    internal class CloseHandle : IInvoke
    {
        string IInvoke.Name => "CloseHandle";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern bool CloseHandle(IntPtr hObject);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool CloseHandle(IntPtr hObject)
        {
            Type[] paramTypes = { typeof(IntPtr) };
            Object[] args = { hObject };

            object res = DynamicPInvokeBuilder(typeof(bool), ""Kernel32.dll"", ""CloseHandle"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
