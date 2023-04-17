using SingleDose.Invokes;

namespace PoisonTendy.Invokes.Comdlg32
{
    internal class ChooseFont : IInvoke
    {
        string IInvoke.Name => "ChooseFont";

        string IInvoke.PInvoke => @"[DllImport(""comdlg32.dll"")]
        public extern static bool ChooseFont(CHOOSEFONT lpcf);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"public static bool ChooseFont(CHOOSEFONT lpcf)
        {
            Type[] paramTypes = { typeof(CHOOSEFONT) };
            Object[] args = { lpcf };
            object res = DynamicPInvokeBuilder(typeof(bool), ""comdlg32.dll"", ""ChooseFont"", ref args, paramTypes);
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
