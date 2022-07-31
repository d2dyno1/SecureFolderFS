﻿using SecureFolderFS.Core.FileSystem.Operations;
using SecureFolderFS.Core.Security;
using SecureFolderFS.Core.Storage;
using SecureFolderFS.Core.Tunnels;
using SecureFolderFS.Core.VaultDataStore;
using SecureFolderFS.Core.VaultDataStore.VaultConfiguration;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;

namespace SecureFolderFS.Core.Instance.Implementation
{
    internal sealed class VaultInstance : IVaultInstance
    {
        public IModifiableFolder VaultFolder { get; internal set; }

        public string VolumeName { get; internal set; }

        public VaultVersion VaultVersion { get; internal set; }

        public ISecureFolderFSInstance SecureFolderFSInstance => SecureFolderFSInstanceImpl;

        public BaseVaultConfiguration BaseVaultConfiguration { get; internal set; }

        public IFileTunnel FileTunnel { get; internal set; }

        public IFolderTunnel FolderTunnel { get; internal set; }

        public IVaultStorageReceiver VaultStorageReceiver { get; internal set; }

        internal SecureFolderFSInstance SecureFolderFSInstanceImpl { get; set; }

        internal ISecurity Security { get; set; }

        internal IFileOperations FileOperations { get; set; }

        internal IDirectoryOperations DirectoryOperations { get; set; }

        public VaultInstance()
        {
            SecureFolderFSInstanceImpl = new SecureFolderFSInstance();
        }

        public void Dispose()
        {
            SecureFolderFSInstance?.Dispose();
            Security?.Dispose();
        }
    }
}
