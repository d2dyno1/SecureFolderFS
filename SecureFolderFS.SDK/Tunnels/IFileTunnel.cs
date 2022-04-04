using SecureFolderFS.SDK.Streams;

namespace SecureFolderFS.SDK.Tunnels
{
    /// <summary>
    /// Provides module for transferring files in and out of vault securely and reliably.
    /// <br/>
    /// <br/>
    /// This API is exposed.
    /// </summary>
    public interface IFileTunnel
    {
        bool TransferFileToVault(string sourcePath, string destinationPath);

        bool TransferFileOutsideOfVault(ICleartextFileStream cleartextFileStream, string destinationPath);
    }
}
