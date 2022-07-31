﻿using System;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.DependencyInjection;
using SecureFolderFS.Sdk.Models;
using SecureFolderFS.Sdk.Services.UserPreferences;
using SecureFolderFS.Sdk.Storage.LocatableStorage;

namespace SecureFolderFS.Sdk.AppModels
{
    /// <inheritdoc cref="IWidgetsContextModel"/>
    public sealed class SavedWidgetsContextModel : IWidgetsContextModel
    {
        private IVaultsSettingsService VaultsSettingsService { get; } = Ioc.Default.GetRequiredService<IVaultsSettingsService>();

        /// <inheritdoc/>
        public IVaultModel VaultModel { get; }

        public SavedWidgetsContextModel(IVaultModel vaultModel)
        {
            VaultModel = vaultModel;
        }

        /// <inheritdoc/>
        public async Task<IWidgetModel?> GetOrCreateWidgetAsync(string widgetId, CancellationToken cancellationToken = default)
        {
            if (VaultModel.Folder is not ILocatableFolder vaultFolder)
                return null;

            VaultsSettingsService.WidgetContexts ??= new();
            VaultsSettingsService.WidgetContexts[vaultFolder.Path].WidgetDataModels ??= new();

            if (!VaultsSettingsService.WidgetContexts[vaultFolder.Path].WidgetDataModels!.TryGetValue(widgetId, out var widgetDataModel))
                return null;

            return null; // TODO: widgetDataModel;
        }

        /// <inheritdoc/>
        public Task<bool> RemoveWidgetAsync(string widgetId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}