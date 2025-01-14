﻿using SecureFolderFS.Sdk.Storage.ExtendableStorage;
using SecureFolderFS.Shared.Helpers;
using SecureFolderFS.Shared.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.Storage.Extensions
{
    public static partial class StorageExtensions
    {
        /// <returns>If successful, returns a <see cref="Stream"/>, otherwise null.</returns>
        /// <inheritdoc cref="IFile.OpenStreamAsync"/>
        public static async Task<Stream?> TryOpenStreamAsync(this IFile file, FileAccess access, CancellationToken cancellationToken = default)
        {
            try
            {
                return await file.OpenStreamAsync(access, cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <returns>Value is <see cref="IResult"/> depending on whether the stream was successfully opened on the file.</returns>
        /// <inheritdoc cref="IFile.OpenStreamAsync"/>
        public static async Task<IResult<Stream?>> OpenStreamWithResultAsync(this IFile file, FileAccess access, CancellationToken cancellationToken = default)
        {
            try
            {
                return new CommonResult<Stream?>(await file.OpenStreamAsync(access, cancellationToken));
            }
            catch (Exception ex)
            {
                return new CommonResult<Stream?>(ex);
            }
        }

        /// <returns>If successful, returns a <see cref="Stream"/>, otherwise null.</returns>
        /// <inheritdoc cref="IFile.OpenStreamAsync"/>
        public static async Task<Stream?> TryOpenStreamAsync(this IFile file, FileAccess access, FileShare share = FileShare.None, CancellationToken cancellationToken = default)
        {
            try
            {
                if (file is IFileExtended fileExtended)
                    return await fileExtended.OpenStreamAsync(access, share, cancellationToken);

                // TODO: Check if the file inherits from ILockableStorable and ensure a disposable handle to it via Stream bridge
                return await file.OpenStreamAsync(access, cancellationToken);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <returns>Value is <see cref="IResult"/> depending on whether the stream was successfully opened on the file.</returns>
        /// <inheritdoc cref="IFile.OpenStreamAsync"/>
        public static async Task<IResult<Stream?>> OpenStreamWithResultAsync(this IFile file, FileAccess access, FileShare share = FileShare.None, CancellationToken cancellationToken = default)
        {
            try
            {
                if (file is IFileExtended fileExtended)
                    return new CommonResult<Stream?>(await fileExtended.OpenStreamAsync(access, share, cancellationToken));

                // TODO: Check if the file inherits from ILockableStorable and ensure a disposable handle to it via Stream bridge
                return new CommonResult<Stream?>(await file.OpenStreamAsync(access, cancellationToken));
            }
            catch (Exception ex)
            {
                return new CommonResult<Stream?>(ex);
            }
        }
    }
}
