namespace SingleDose.Invokes.Comdlg32
{
    internal class GetOpenFileName : IInvoke
    {
        string IInvoke.Name => "GetOpenFileName";

        string IInvoke.PInvoke => @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool GetOpenFileName([In, Out] OpenFileName ofn)
        {
            Type[] paramTypes = { typeof(OpenFileName) };
            Object[] args = { ofn };
            object res = DynamicPInvokeBuilder(typeof(bool), ""Comdlg32.dll"", ""GetOpenFileName"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";

    }
}
