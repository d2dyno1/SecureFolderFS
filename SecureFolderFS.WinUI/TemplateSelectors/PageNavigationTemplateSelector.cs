using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SecureFolderFS.Backend.ViewModels.Pages;

namespace SecureFolderFS.WinUI.TemplateSelectors
{
    internal sealed class PageNavigationTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LoginPageDataTemplate { get; set; }

        public DataTemplate DashboardPageDataTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return item switch
            {
                VaultLoginPageViewModel => LoginPageDataTemplate,
                VaultDashboardPageViewModel => DashboardPageDataTemplate,
                _ => base.SelectTemplateCore(item, container)
            };
        }
    }
}
