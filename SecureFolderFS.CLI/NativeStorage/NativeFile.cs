﻿using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.ExtendableStorage;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.WinUI.Storage.NativeStorage
{
    /// <inheritdoc cref="IFile"/>
    internal sealed class NativeFile : NativeStorable, ILocatableFile, IModifiableFile, IFileExtended
    {
        public NativeFile(string path)
            : base(path)
        {
        }

        /// <inheritdoc/>
        public Task<Stream> OpenStreamAsync(FileAccess access, CancellationToken cancellationToken = default)
        {
            return OpenStreamAsync(access, FileShare.None, cancellationToken);
        }

        /// <inheritdoc/>
        public Task<Stream> OpenStreamAsync(FileAccess access, FileShare share = FileShare.None, CancellationToken cancellationToken = default)
        {
            var stream = File.Open(Path, FileMode.Open, access, share);
            return Task.FromResult<Stream>(stream);
        }
    }
}
