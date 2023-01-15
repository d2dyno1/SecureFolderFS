﻿using SecureFolderFS.Core.Cryptography;
using SecureFolderFS.Core.FileSystem.Analytics;
using SecureFolderFS.Core.FileSystem.FileNames;
using System;
using System.IO;

namespace SecureFolderFS.Core.FileNames
{
    /// <inheritdoc cref="IFileNameAccess"/>
    internal class InstantFileNameAccess : IFileNameAccess
    {
        protected readonly Security security;
        protected readonly IFileSystemStatistics? fileSystemStatistics;

        public InstantFileNameAccess(Security security, IFileSystemStatistics? fileSystemStatistics)
        {
            this.security = security;
            this.fileSystemStatistics = fileSystemStatistics;
        }

        /// <inheritdoc/>
        public virtual ReadOnlySpan<char> GetCleartextName(ReadOnlySpan<char> ciphertextName, ReadOnlySpan<byte> directoryId)
        {
            fileSystemStatistics?.NotifyFileNameAccess();

            var nameWithoutExt = Path.GetFileNameWithoutExtension(ciphertextName);
            if (nameWithoutExt.IsEmpty)
                return ReadOnlySpan<char>.Empty;

            var cleartextName = security.NameCrypt!.DecryptName(nameWithoutExt, directoryId);
            if (cleartextName is null)
                return ReadOnlySpan<char>.Empty;

            return cleartextName;
        }

        /// <inheritdoc/>
        public virtual ReadOnlySpan<char> GetCiphertextName(ReadOnlySpan<char> cleartextName, ReadOnlySpan<byte> directoryId)
        {
            fileSystemStatistics?.NotifyFileNameAccess();

            var ciphertextName = security.NameCrypt!.EncryptName(cleartextName, directoryId);
            return Path.ChangeExtension(ciphertextName, Constants.ENCRYPTED_FILE_EXTENSION);
        }
    }
}
