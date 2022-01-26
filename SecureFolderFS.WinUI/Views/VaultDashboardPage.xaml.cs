using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using SecureFolderFS.Backend.Messages;
using SecureFolderFS.Backend.Models;
using SecureFolderFS.Backend.ViewModels.Pages;
using SecureFolderFS.Backend.ViewModels.Pages.DashboardPages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable enable

namespace SecureFolderFS.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VaultDashboardPage : Page, IRecipient<NavigationFinishedMessage>, IRecipient<DashboardNavigationRequestedMessage>
    {
        public VaultDashboardPage()
        {
            this.InitializeComponent();

            WeakReferenceMessenger.Default.Register<NavigationFinishedMessage>(this);
        }

        public void Receive(DashboardNavigationRequestedMessage message)
        {
            NavigatePage(message.Value);
        }

        private void NavigatePage(BaseDashboardPageViewModel baseDashboardPageViewModel)
        {
            switch (baseDashboardPageViewModel)
            {
                case VaultMainDashboardPageViewModel:
                    ContentFrame.Navigate(typeof(VaultMainDashboardPage), new DashboardPageNavigationParameterModel() { ViewModel = baseDashboardPageViewModel }, new SlideNavigationTransitionInfo());
                    break;
            }
        }

        public VaultDashboardPageViewModel ViewModel
        {
            get => (VaultDashboardPageViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(VaultDashboardPageViewModel), typeof(VaultDashboardPage), new PropertyMetadata(null));

        public void Receive(NavigationFinishedMessage message)
        {
            if (ViewModel != message.Value && message.Value is VaultDashboardPageViewModel vaultDashboardPageViewModel)
            {
                ViewModel = vaultDashboardPageViewModel;
                this.Bindings.Update();
            }
        }
    }
}
