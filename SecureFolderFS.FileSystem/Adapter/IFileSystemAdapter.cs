using System;

namespace SecureFolderFS.FileSystem.Adapter
{
    public interface IFileSystemAdapter : IDisposable
    {
        void InitializeFileSystem();

        void StartFileSystem(string mountLocation);

        bool StopFileSystem(string mountLocation);
    }
}
