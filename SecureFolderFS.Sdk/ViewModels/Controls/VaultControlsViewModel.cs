﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SecureFolderFS.Sdk.Messages;
using SecureFolderFS.Sdk.Messages.Navigation;
using SecureFolderFS.Sdk.Services;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using SecureFolderFS.Sdk.ViewModels.Pages.Vault;
using SecureFolderFS.Sdk.ViewModels.Pages.Vault.Dashboard;
using SecureFolderFS.Sdk.ViewModels.Vault;
using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.ViewModels.Controls
{
    public sealed partial class VaultControlsViewModel : ObservableObject
    {
        private readonly IMessenger _messenger;
        private readonly UnlockedVaultViewModel _unlockedVaultViewModel;

        private IFileExplorerService FileExplorerService { get; } = Ioc.Default.GetRequiredService<IFileExplorerService>();

        public VaultControlsViewModel(IMessenger messenger, UnlockedVaultViewModel unlockedVaultViewModel)
        {
            _messenger = messenger;
            _unlockedVaultViewModel = unlockedVaultViewModel;
        }

        [RelayCommand(AllowConcurrentExecutions = true)]
        private async Task ShowInFileExplorerAsync()
        {
            if (_unlockedVaultViewModel.UnlockedVaultModel.RootFolder is not ILocatableFolder rootFolder)
                return;

            await FileExplorerService.OpenInFileExplorerAsync(rootFolder);
        }

        [RelayCommand]
        private async Task LockVaultAsync()
        {
            await _unlockedVaultViewModel.UnlockedVaultModel.LockAsync();

            var loginPageViewModel = new VaultLoginPageViewModel(_unlockedVaultViewModel.VaultViewModel);
            _ = loginPageViewModel.InitAsync();

            WeakReferenceMessenger.Default.Send(new VaultLockedMessage(_unlockedVaultViewModel.VaultViewModel.VaultModel));
            WeakReferenceMessenger.Default.Send(new NavigationRequestedMessage(loginPageViewModel));
        }

        [RelayCommand]
        private void OpenProperties()
        {
            _messenger.Send(new NavigationRequestedMessage(new VaultPropertiesPageViewModel(_unlockedVaultViewModel, _messenger)));
        }
    }
}
