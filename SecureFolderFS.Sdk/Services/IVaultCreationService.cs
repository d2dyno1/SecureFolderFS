﻿using SecureFolderFS.Sdk.AppModels;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;
using SecureFolderFS.Shared.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.Services
{
    /// <summary>
    /// Represents a service used for vault creation routine.
    /// </summary>
    public interface IVaultCreationService : IDisposable
    {
        /// <summary>
        /// Sets the root <see cref="IModifiableFolder"/> that represents the vault.
        /// </summary>
        /// <param name="folder">The folder of the vault to be set.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. If <paramref name="folder"/> was set successfully, returns true, otherwise false.</returns>
        Task<bool> SetVaultFolderAsync(IModifiableFolder folder, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the password used for decryption of this vault.
        /// </summary>
        /// <param name="password">The password key.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. If successful, returns true, otherwise false.</returns>
        Task<bool> SetPasswordAsync(IPassword password, CancellationToken cancellationToken = default);

        /// <summary>
        /// Prepares a new configuration file for vault.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is <see cref="IResult"/> depending on whether the configuration file was created successfully.</returns>
        Task<IResult> PrepareConfigurationAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Prepares a new keystore using <paramref name="keystoreStream"/>.
        /// </summary>
        /// <param name="keystoreStream">The stream where keystore will be stored.</param>
        /// <param name="serializer">The serializer used to serialize the keystore.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is <see cref="IResult"/> depending on whether the keystore was created successfully.</returns>
        Task<IResult> PrepareKeystoreAsync(Stream keystoreStream, IAsyncSerializer<Stream> serializer, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets the content and filename cipher scheme that will be used for vault encryption.
        /// </summary>
        /// <param name="nameCipher">The filename cipher scheme to set.</param>
        /// <param name="contentCipher">The content cipher scheme to set.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is <see cref="IResult"/> depending on whether cipher scheme was set successfully..</returns>
        Task<IResult> SetCipherSchemeAsync(CipherInfoModel contentCipher, CipherInfoModel nameCipher, CancellationToken cancellationToken = default);

        /// <summary>
        /// Finalizes and deploys the routine that will finish the vault creation task.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. Value is <see cref="IResult"/> that represents the action.</returns>
        Task<IResult> DeployAsync(CancellationToken cancellationToken = default);
    }
}
