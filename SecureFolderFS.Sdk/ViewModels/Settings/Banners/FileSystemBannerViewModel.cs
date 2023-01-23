﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Services.UserPreferences;
using SecureFolderFS.Shared.Utils;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Settings.Banners
{
    public sealed class FileSystemBannerViewModel : ObservableObject, IAsyncInitialize
    {
        private IVaultService VaultService { get; } = Ioc.Default.GetRequiredService<IVaultService>();

        private IPreferencesSettingsService PreferencesSettingsService { get; } = Ioc.Default.GetRequiredService<IPreferencesSettingsService>();

        public ObservableCollection<FileSystemAdapterItemViewModel> FileSystemAdapters { get; }

        public string? PreferredFileSystemId
        {
            get => PreferencesSettingsService.PreferredFileSystemId;
            set => PreferencesSettingsService.PreferredFileSystemId = value;
        }

        public FileSystemBannerViewModel()
        {
            FileSystemAdapters = new();
        }

        public Task InitAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in VaultService.GetFileSystems())
                FileSystemAdapters.Add(new(item));

            return Task.CompletedTask;
        }
    }
}
