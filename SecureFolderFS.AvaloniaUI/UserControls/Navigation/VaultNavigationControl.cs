using System;
using System.Collections.Generic;
using FluentAvalonia.UI.Media.Animation;
using SecureFolderFS.AvaloniaUI.Views.Vault;
using SecureFolderFS.Sdk.ViewModels.Pages.Vault;
using SecureFolderFS.Sdk.ViewModels.Vault;

namespace SecureFolderFS.AvaloniaUI.UserControls.Navigation
{
    /// <inheritdoc cref="NavigationControl"/>
    internal sealed class VaultNavigationControl : NavigationControl
    {
        public Dictionary<VaultViewModel, BaseVaultPageViewModel> NavigationCache { get; }

        public VaultNavigationControl()
        {
            NavigationCache = new();
        }

        public override void Navigate<TViewModel>(TViewModel viewModel, NavigationTransitionInfo? transitionInfo)
        {
            if (viewModel is not BaseVaultPageViewModel pageViewModel)
                throw new ArgumentException($"{nameof(viewModel)} does not inherit from {nameof(BaseVaultPageViewModel)}.");

            // TODO Dashboard closing animation
            // if (pageViewModel is VaultLoginPageViewModel && (NavigationCache.TryGetValue(pageViewModel.VaultViewModel, out var existing)) && existing is VaultDashboardPageViewModel)
            //    transitionInfo ??= new ContinuumNavigationTransitionInfo();

            // Standard animation
            transitionInfo ??= new EntranceNavigationTransitionInfo();

            // Set or update the view model for individual page
            NavigationCache[pageViewModel.VaultViewModel] = pageViewModel;

            var pageType = viewModel switch
            {
                VaultLoginPageViewModel => typeof(VaultLoginPage),
                VaultDashboardPageViewModel => typeof(VaultDashboardPage),
                _ => throw new ArgumentNullException(nameof(viewModel))
            };

            ContentFrame.Navigate(pageType, viewModel, transitionInfo);
        }

        public void ClearContent()
        {
            ContentFrame.Content = null;
        }
    }
}