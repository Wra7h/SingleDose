namespace SingleDose.Invokes
{
    public interface IInvoke
    {
        string Name { get; }

        string PInvoke { get; }

        string DInvoke { get; }
    }
}
