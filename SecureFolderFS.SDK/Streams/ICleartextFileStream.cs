namespace SecureFolderFS.SDK.Streams
{
    public interface ICleartextFileStream : IBaseFileStream
    {
        bool CanBeDeleted();
    }
}
