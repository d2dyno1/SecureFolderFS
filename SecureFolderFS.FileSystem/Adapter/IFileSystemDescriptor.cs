using SecureFolderFS.FileSystem.Enums;

namespace SecureFolderFS.FileSystem.Adapter
{
    public interface IFileSystemDescriptor
    {
        string ProviderName { get; }

        FileSystemAvailabilityErrorType IsFileSystemAvailable();
    }
}
