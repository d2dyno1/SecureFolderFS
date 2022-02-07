﻿using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using SecureFolderFS.Backend.Messages;
using SecureFolderFS.Backend.Models.Transitions;
using SecureFolderFS.Backend.Utils;
using SecureFolderFS.Backend.ViewModels.Dashboard.Navigation;
using SecureFolderFS.Backend.ViewModels.Pages;
using SecureFolderFS.Backend.ViewModels.Pages.DashboardPages;
using SecureFolderFS.WinUI.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable enable

namespace SecureFolderFS.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VaultDashboardPage : Page, IRecipient<DashboardNavigationFinishedMessage>
    {
        public VaultDashboardPageViewModel ViewModel
        {
            get => (VaultDashboardPageViewModel)DataContext;
            set => DataContext = value;
        }

        public VaultDashboardPage()
        {
            this.InitializeComponent();
        }

        public void Receive(DashboardNavigationFinishedMessage message)
        {
            NavigatePage(message.Value, message.Transition);
        }

        private void NavigatePage(BaseDashboardPageViewModel baseDashboardPageViewModel, TransitionModel? transition = null)
        {
            var transitionInfo = ConversionHelpers.ToNavigationTransitionInfo(transition);
            switch (baseDashboardPageViewModel)
            {
                case VaultMainDashboardPageViewModel:
                    ContentFrame.Navigate(typeof(VaultMainDashboardPage), baseDashboardPageViewModel, transitionInfo ?? new SuppressNavigationTransitionInfo());
                    break;

                case VaultDashboardPropertiesPageViewModel:
                    ContentFrame.Navigate(typeof(VaultDashboardPropertiesPage), baseDashboardPageViewModel, transitionInfo ?? new SuppressNavigationTransitionInfo());
                    break;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is VaultDashboardPageViewModel viewModel)
            {
                ViewModel = viewModel;
                ViewModel.Messenger.Register<DashboardNavigationFinishedMessage>(this);
                ViewModel.StartNavigation();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            (ViewModel as ICleanable)?.Cleanup();

            ViewModel.Messenger.Unregister<DashboardNavigationFinishedMessage>(this);

            base.OnNavigatingFrom(e);
        }

        private void BreadcrumbBar_ItemClicked(BreadcrumbBar sender, BreadcrumbBarItemClickedEventArgs args)
        {
            if (args.Item is NavigationItemViewModel itemViewModel)
            {
                itemViewModel.NavigationAction?.Invoke(ViewModel.NavigationBreadcrumbViewModel.DashboardNavigationItems.FirstOrDefault());
            }
        }
    }
}
