namespace SingleDose.Invokes.Setupapi
{
    internal class SetupOpenFileQueue : IInvoke
    {
        string IInvoke.Name => "SetupOpenFileQueue";

        string IInvoke.PInvoke => @"[DllImport(""Setupapi.dll"")]
        static extern IntPtr SetupOpenFileQueue();

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static IntPtr SetupOpenFileQueue()
        {
            Type[] paramTypes = { };
            Object[] args = { };
            object res = DynamicPInvokeBuilder(typeof(IntPtr), ""Setupapi.dll"", ""SetupOpenFileQueue"", ref args, paramTypes);
            return (IntPtr)res;
        }

        {{INVOKE}}";
    }
}
