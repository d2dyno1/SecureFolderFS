﻿using CommunityToolkit.Mvvm.DependencyInjection;
using SecureFolderFS.Sdk.Models;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Services.UserPreferences;
using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.Extensions;
using SecureFolderFS.Shared.Helpers;
using SecureFolderFS.Shared.Utils;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.AppModels
{
    /// <inheritdoc cref="IVaultUnlockingModel"/>
    public sealed class VaultUnlockingModel : IVaultUnlockingModel
    {
        private IVaultService VaultService { get; } = Ioc.Default.GetRequiredService<IVaultService>();

        private IPreferencesSettingsService PreferencesSettingsService { get; } = Ioc.Default.GetRequiredService<IPreferencesSettingsService>();

        private IVaultUnlockingService VaultUnlockingService { get; } = Ioc.Default.GetRequiredService<IVaultUnlockingService>();

        /// <inheritdoc/>
        public async Task<IResult> SetFolderAsync(IFolder folder, CancellationToken cancellationToken = default)
        {
            // TODO: Maybe use IAsyncValidator<IFolder>
            var vaultFolderResult = await VaultUnlockingService.SetVaultFolderAsync(folder, cancellationToken);
            if (!vaultFolderResult.Successful)
                return vaultFolderResult;

            var configFileResult = await folder.GetFileWithResultAsync(Core.Constants.VAULT_CONFIGURATION_FILENAME, cancellationToken);
            if (!configFileResult.Successful)
                return configFileResult;

            var configStreamResult = await configFileResult.Value!.OpenStreamWithResultAsync(FileAccess.Read, FileShare.Read, cancellationToken);
            if (!configStreamResult.Successful)
                return configStreamResult;

            await using (configStreamResult.Value)
            {
                var setStreamResult = await VaultUnlockingService.SetConfigurationStreamAsync(configStreamResult.Value!, cancellationToken);
                if (!setStreamResult.Successful)
                    return setStreamResult;
            }

            return CommonResult.Success;
        }

        /// <inheritdoc/>
        public async Task<IResult> SetKeystoreAsync(IKeystoreModel keystoreModel, CancellationToken cancellationToken = default)
        {
            var keystoreStreamResult = await keystoreModel.GetKeystoreStreamAsync(FileAccess.Read, cancellationToken);
            if (!keystoreStreamResult.Successful)
                return keystoreStreamResult;

            return await VaultUnlockingService.SetKeystoreStreamAsync(keystoreStreamResult.Value!, keystoreModel.KeystoreSerializer, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<IResult<IUnlockedVaultModel?>> UnlockAsync(IPassword password, CancellationToken cancellationToken = default)
        {
            // Get file system
            var fileSystem = VaultService.GetFileSystemById(PreferencesSettingsService.PreferredFileSystemId);
            if (fileSystem is null)
                return new CommonResult<IUnlockedVaultModel?>(new ArgumentException($"File System descriptor '{PreferencesSettingsService.PreferredFileSystemId}' was not found."));

            var fileSystemResult = await VaultUnlockingService.SetFileSystemAsync(fileSystem, cancellationToken);
            if (!fileSystemResult.Successful)
                return new CommonResult<IUnlockedVaultModel?>(fileSystemResult.Exception);

            return await VaultUnlockingService.UnlockAndStartAsync(password, cancellationToken);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            VaultUnlockingService.Dispose();
        }
    }
}
