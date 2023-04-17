using SingleDose.Invokes;

namespace PoisonTendy.Invokes.OleDlg
{
    internal class OleUIBusy : IInvoke
    {
        string IInvoke.Name => "OleUIBusy";

        string IInvoke.PInvoke => @"[DllImport(""OleDlg.dll"")]
        public extern static bool OleUIBusy(ref OLEUIBUSY unnamedParam1);

        {{INVOKE}}";

        string IInvoke.DInvoke => @"static bool OleUIBusy(ref OLEUIBUSY unnamedParam1)
        {
            Type[] paramTypes = { typeof(OLEUIBUSY) };
            Object[] args = { unnamedParam1 };
            object res = DynamicPInvokeBuilder(typeof(bool), ""OleDlg.dll"", ""OleUIBusy"", ref args, paramTypes);
            unnamedParam1 = (OLEUIBUSY)args[0];
            return (bool)res;
        }

        {{INVOKE}}";
    }
}
