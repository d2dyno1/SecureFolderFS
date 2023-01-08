using SecureFolderFS.Core.Cryptography;
using SecureFolderFS.Core.FileSystem;
using SecureFolderFS.Core.FileSystem.AppModels;
using SecureFolderFS.Core.FileSystem.Directories;
using SecureFolderFS.Core.FileSystem.Paths;
using SecureFolderFS.Core.FileSystem.Storage;
using SecureFolderFS.Core.FileSystem.Streams;
using SecureFolderFS.Core.FUSE.AppModels;
using SecureFolderFS.Core.FUSE.Callbacks;
using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.LocatableStorage;

namespace SecureFolderFS.Core.FUSE.Mounters
{
    /// <inheritdoc cref="IMountableFileSystem"/>
    public sealed class FuseMountable : IMountableFileSystem
    {
        private readonly FuseWrapper _fuseWrapper;
        private readonly string _vaultName;

        private FuseMountable(OnDeviceFuse fuseCallbacks, string vaultName)
        {
            _fuseWrapper = new(fuseCallbacks);
            _vaultName = vaultName;
        }

        /// <inheritdoc/>
        public Task<IVirtualFileSystem> MountAsync(MountOptions mountOptions, CancellationToken cancellationToken = default)
        {
            if (mountOptions is not FuseMountOptions fuseMountOptions)
                throw new ArgumentException($"Parameter {nameof(mountOptions)} does not implement {nameof(FuseMountOptions)}.");

            var mountPath = fuseMountOptions.MountPath;
            if (mountPath == null)
            {
                mountPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    nameof(SecureFolderFS), _vaultName);

                var i = 1;
                while (Directory.Exists(mountPath)) // TODO Check if a filesystem is already mounted inside
                {
                    mountPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                        nameof(SecureFolderFS), $"{_vaultName} ({i++})");
                }
            }

            _fuseWrapper.StartFileSystem(mountPath);
            var fuseFileSystem = new FuseFileSystem(_fuseWrapper, new SimpleFolder(mountPath));

            return Task.FromResult<IVirtualFileSystem>(fuseFileSystem);
        }

        public static IMountableFileSystem CreateMountable(string vaultName, IPathConverter pathConverter, IFolder contentFolder, Security security, IDirectoryIdAccess directoryIdAccess, IStreamsAccess streamsAccess)
        {
            if (contentFolder is not ILocatableFolder locatableContentFolder)
                throw new ArgumentException("The vault content folder is not locatable.");

            var fuseCallbacks = new OnDeviceFuse(pathConverter, new(streamsAccess))
            {
                LocatableContentFolder = locatableContentFolder,
                Security = security,
                DirectoryIdAccess = directoryIdAccess
            };

            return new FuseMountable(fuseCallbacks, vaultName);
        }
    }
}