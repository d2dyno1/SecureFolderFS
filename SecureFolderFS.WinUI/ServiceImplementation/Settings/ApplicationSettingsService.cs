﻿using SecureFolderFS.Sdk.Services.Settings;
using SecureFolderFS.Sdk.AppModels;
using SecureFolderFS.Sdk.Storage.StoragePool;
using System;

namespace SecureFolderFS.WinUI.ServiceImplementation.Settings
{
    /// <inheritdoc cref="IApplicationSettingsService"/>
    internal sealed class ApplicationSettingsService : SingleFileSettingsModel, IApplicationSettingsService
    {
        public ApplicationSettingsService(IFilePool? settingsFilePool)
        {
            FilePool = settingsFilePool;
            SettingsDatabase = new DictionarySettingsDatabaseModel(JsonToStreamSerializer.Instance);
            IsAvailable = settingsFilePool is not null;
        }

        /// <inheritdoc/>
        protected override string? SettingsStorageName { get; } = Constants.LocalSettings.APPLICATION_SETTINGS_FILENAME;

        /// <inheritdoc/>
        public DateTime UpdateLastChecked
        {
            get => GetSetting<DateTime>(() => new());
            set => SetSetting<DateTime>(value);
        }
    }
}
