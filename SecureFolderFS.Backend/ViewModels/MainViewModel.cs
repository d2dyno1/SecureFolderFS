using CommunityToolkit.Mvvm.ComponentModel;
using SecureFolderFS.Backend.ViewModels.Pages;
using SecureFolderFS.Backend.ViewModels.Sidebar;

#nullable enable

namespace SecureFolderFS.Backend.ViewModels
{
    public sealed class MainViewModel : ObservableObject
    {
        private BasePageViewModel? _ActivePageViewModel;
        public BasePageViewModel? ActivePageViewModel
        {
            get => _ActivePageViewModel;
            set => SetProperty(ref _ActivePageViewModel, value);
        }

        public SidebarViewModel SidebarViewModel { get; }

        public MainViewModel()
        {
            SidebarViewModel = new();
        }
    }
}
