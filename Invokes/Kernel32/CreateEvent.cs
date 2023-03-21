namespace SingleDose.Invokes.Kernel32
{
    internal class CreateEvent : IInvoke
    {
        string IInvoke.Name => "CreateEvent";

        string IInvoke.PInvoke => @"[DllImport(""kernel32.dll"")]
        static extern IntPtr CreateEvent(
            IntPtr lpEventAttributes, bool bManualReset,
            bool bInitialState, string lpName);
            
        {{INVOKE}}";

        string IInvoke.DInvoke => @"static IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName)
        {
            Type[] paramTypes = { typeof(IntPtr), typeof(bool), typeof(bool), typeof(string) };
            Object[] args = { lpEventAttributes, bManualReset, bInitialState, lpName };

            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Kernel32.dll"", ""CreateEvent"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
