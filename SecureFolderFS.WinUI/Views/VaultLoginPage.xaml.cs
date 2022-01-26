using Windows.System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using SecureFolderFS.Backend.ViewModels.Pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable enable

namespace SecureFolderFS.WinUI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VaultLoginPage : Page
    {
        public VaultLoginPage()
        {
            this.InitializeComponent();
        }

        private void VaultPasswordBox_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                ViewModel.UnlockVaultCommand.Execute((sender as PasswordBox)!.Password);
            }
        }

        public VaultLoginPageViewModel ViewModel
        {
            get => (VaultLoginPageViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(VaultLoginPageViewModel), typeof(VaultLoginPage), new PropertyMetadata(null));
    }
}
