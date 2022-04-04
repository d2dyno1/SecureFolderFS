using System;
using SecureFolderFS.SDK.Paths;
using SecureFolderFS.SDK.Storage;
using SecureFolderFS.SDK.Tunnels;
using SecureFolderFS.SDK.VaultStore;
using SecureFolderFS.SDK.VaultStore.VaultConfiguration;

namespace SecureFolderFS.SDK.Instance
{
    /// <summary>
    /// Provides module for managing the vault instance.
    /// <br/>
    /// <br/>
    /// This API is exposed.
    /// </summary>
    public interface IVaultInstance : IDisposable
    {
        VaultPath VaultPath { get; }

        string VolumeName { get; }

        VaultVersion VaultVersion { get; }

        ISecureFolderFSInstance SecureFolderFSInstance { get; }

        BaseVaultConfiguration BaseVaultConfiguration { get; }

        IFileTunnel FileTunnel { get; }

        IFolderTunnel FolderTunnel { get; }

        IVaultStorageReceiver VaultStorageReceiver { get; }
    }
}
