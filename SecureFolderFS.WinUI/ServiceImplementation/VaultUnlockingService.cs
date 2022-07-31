﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using SecureFolderFS.Core.Instance;
using SecureFolderFS.Core.Routines;
using SecureFolderFS.Core.VaultDataStore;
using SecureFolderFS.Core.VaultDataStore.VaultConfiguration;
using SecureFolderFS.Core.VaultLoader.Routine;
using SecureFolderFS.Sdk.Models;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.ModifiableStorage;
using SecureFolderFS.Shared.Utils;
using SecureFolderFS.WinUI.AppModels;

namespace SecureFolderFS.WinUI.ServiceImplementation
{
    internal sealed class VaultUnlockingService : IVaultUnlockingService
    {
        private IModifiableFolder? _vaultFolder;
        private IVaultLoadRoutineStep3? _step3;
        private IVaultLoadRoutineStep8? _step8;

        private IFileSystemService FileSystemService { get; } = Ioc.Default.GetRequiredService<IFileSystemService>();

        /// <inheritdoc/>
        public Task<bool> SetVaultFolderAsync(IModifiableFolder folder, CancellationToken cancellationToken = default)
        {
            _vaultFolder = folder;

            _step3 = VaultRoutines.NewVaultLoadRoutine()
                .EstablishRoutine()
                .SetFolder(folder)
                .AddFileOperations();

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task<bool> IsSupportedAsync(CancellationToken cancellationToken = default)
        {
            if (_vaultFolder is null)
                return false;

            var configFile = await _vaultFolder.GetFileAsync(Core.Constants.VAULT_CONFIGURATION_FILENAME);
            if (configFile is null)
                return false;

            await using var configStream = await configFile.OpenStreamAsync(FileAccess.Read, FileShare.Read);
            if (configStream is null)
                return false;
            
            var rawConfiguration = RawVaultConfiguration.Load(configStream);
            return VaultVersion.IsVersionSupported(rawConfiguration);
        }

        /// <inheritdoc/>
        public Task<bool> SetKeystoreStreamAsync(Stream stream, IAsyncSerializer<Stream> serializer, CancellationToken cancellationToken = default)
        {
            _ = _step3 ?? throw new InvalidOperationException("The vault folder has not been set yet.");

            _step8 = _step3.FindConfigurationFile()
                .ContinueConfigurationFileInitialization()
                .FindKeystoreFile(true, new StreamKeystoreDiscoverer(stream))
                .ContinueKeystoreFileInitialization()
                .AddEncryptionAlgorithmBuilder();

            return Task.FromResult(true);
        }

        /// <inheritdoc/>
        public async Task<IUnlockedVaultModel?> UnlockAndStartAsync(IPassword password, CancellationToken cancellationToken = default)
        {
            _ = _step8 ?? throw new InvalidOperationException("The keystore has not been set yet.");

            IVaultInstance? vaultInstance = null;
            try
            {
                vaultInstance = _step8.DeriveMasterKeyFromPassword(password)
                    .ContinueInitializationWithMasterKey()
                    .VerifyVaultConfiguration()
                    .ContinueInitialization()
                    .Finalize()
                    .Deploy();

                if (vaultInstance is null)
                    return null;

                vaultInstance.SecureFolderFSInstance.StartFileSystem();
                await Task.Delay(100, cancellationToken); // Wait for the file system to start

                var rootFolder = await FileSystemService.GetFolderFromPathAsync(vaultInstance.SecureFolderFSInstance.MountLocation);
                return new VaultInstanceUnlockedVaultModel(vaultInstance, rootFolder!);
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}