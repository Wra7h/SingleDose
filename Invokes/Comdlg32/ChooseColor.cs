namespace SingleDose.Invokes.Comdlg32
{
    internal class ChooseColor : IInvoke
    {
        string IInvoke.Name => "ChooseColor";

        string IInvoke.PInvoke => @"[DllImport(""comdlg32.dll"", SetLastError = true, CharSet = CharSet.Auto)]
        public extern static bool ChooseColor(ref CHOOSECOLOR lpcc);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool ChooseColor(ref CHOOSECOLOR lpcc)
        {
            Type[] paramTypes = { Type.GetType(typeof(CHOOSECOLOR)+ ""&"") };
            Object[] args = { lpcc };
            object res = DynamicPInvokeBuilder(typeof(bool), ""Comdlg32.dll"", ""ChooseColor"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
