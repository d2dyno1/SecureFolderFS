using System;
using SecureFolderFS.SDK.Paths;
using SecureFolderFS.Core.Storage;
using SecureFolderFS.SDK.Tunnels;

namespace SecureFolderFS.Core.Tunnels.Implementation
{
    internal sealed class FolderTunnel : IFolderTunnel
    {
        public bool TransferFolderOutsideOfVault(IVaultFolder vaultFolder, string destinationPath)
        {
            throw new NotImplementedException();
        }

        public IVaultFolder TransferFolderToVault(string sourcePath, ICleartextPath destinationCleartextPath)
        {
            throw new NotImplementedException();
        }
    }
}
