namespace Replace.Common.PaddedString
{
    public interface IPaddedString : Unmanaged.IMarshalled
    {
        string Value { get; set; }
        int Padding { get; }
    }
}