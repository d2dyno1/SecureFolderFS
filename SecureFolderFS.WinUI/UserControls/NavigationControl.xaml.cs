using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SecureFolderFS.Backend.Extensions;
using SecureFolderFS.Backend.Messages;
using SecureFolderFS.Backend.Models;
using SecureFolderFS.Backend.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SecureFolderFS.WinUI.UserControls
{
    public sealed partial class NavigationControl : UserControl, IRecipient<NavigationRequestedMessage>, IRecipient<RemoveVaultRequestedMessage>, IRecipient<AddVaultRequestedMessage>
    {
        private Dictionary<VaultModel, BasePageViewModel?> NavigationDestinations { get; }

        public NavigationControl()
        {
            this.InitializeComponent();

            this.NavigationDestinations = new();

            WeakReferenceMessenger.Default.Register<NavigationRequestedMessage>(this);
            WeakReferenceMessenger.Default.Register<RemoveVaultRequestedMessage>(this);
            WeakReferenceMessenger.Default.Register<AddVaultRequestedMessage>(this);
        }

        public void Receive(RemoveVaultRequestedMessage message)
        {
            NavigationDestinations.Remove(message.Value, out var viewModel);
            viewModel?.Dispose();
        }

        public void Receive(AddVaultRequestedMessage message)
        {
            NavigationDestinations.AddOrSet(message.Value);
        }

        public void Receive(NavigationRequestedMessage message)
        {
            if (message.Value == null)
            {
                NavigationDestinations.SetAndGet(message.VaultModel, out var basePageViewModel, () => new VaultLoginPageViewModel(message.VaultModel));
                PageViewModel = basePageViewModel;
            }
            else
            {
                if (!NavigationDestinations.SetAndGet(message.VaultModel, out _, () => message.Value))
                {
                    // Wasn't updated, do it manually..
                    NavigationDestinations[message.VaultModel] = message.Value;
                    PageViewModel = message.Value;
                }
            }

            WeakReferenceMessenger.Default.Send(new NavigationFinishedMessage(PageViewModel!));
        }

        public BasePageViewModel PageViewModel
        {
            get => (BasePageViewModel)GetValue(PageViewModelProperty);
            set => SetValue(PageViewModelProperty, value);
        }
        public static readonly DependencyProperty PageViewModelProperty =
            DependencyProperty.Register("PageViewModel", typeof(BasePageViewModel), typeof(NavigationControl), new PropertyMetadata(null));
    }
}
