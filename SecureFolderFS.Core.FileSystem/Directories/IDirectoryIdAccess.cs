﻿using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Core.FileSystem.Directories
{
    /// <summary>
    /// Accesses directory IDs found on the encrypting file system.
    /// </summary>
    public interface IDirectoryIdAccess
    {
        /// <summary>
        /// Gets the directory ID of provided <paramref name="ciphertextPath"/> directory path.
        /// </summary>
        /// <param name="ciphertextPath">The path to the ciphertext directory.</param>
        /// <returns>Value is <see cref="DirectoryId"/> if the directory ID was retrieved successfully, otherwise null.</returns>
        DirectoryId? GetDirectoryId(string ciphertextPath);

        /// <summary>
        /// Sets the directory ID of provided <paramref name="ciphertextPath"/> directory path.
        /// </summary>
        /// <param name="ciphertextPath">The path to the ciphertext directory.</param>
        /// <param name="directoryId">The <see cref="DirectoryId"/> to set for the directory.</param>
        /// <returns>Value is true if directory ID was successfully set, otherwise false.</returns>
        bool SetDirectoryId(string ciphertextPath, DirectoryId directoryId);

        /// <summary>
        /// Gets the directory ID of provided <paramref name="ciphertextPath"/> directory path.
        /// </summary>
        /// <param name="ciphertextPath">The path to the ciphertext directory.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is <see cref="DirectoryId"/> if the directory ID was retrieved successfully, otherwise null.</returns>
        Task<DirectoryId?> GetDirectoryIdAsync(string ciphertextPath, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the directory ID of provided <paramref name="ciphertextPath"/> directory path.
        /// </summary>
        /// <param name="ciphertextPath">The path to the ciphertext directory.</param>
        /// <param name="directoryId">The <see cref="DirectoryId"/> to set for the directory.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is true if directory ID was successfully set, otherwise false.</returns>
        Task<bool> SetDirectoryIdAsync(string ciphertextPath, DirectoryId directoryId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes associated directory ID from the list of known IDs.
        /// </summary>
        /// <param name="ciphertextPath">The path associated with a directory ID.</param>
        void RemoveDirectoryId(string ciphertextPath);
    }
}