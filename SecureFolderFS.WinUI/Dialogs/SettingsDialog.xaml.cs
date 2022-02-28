﻿using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using SecureFolderFS.Backend.Dialogs;
using SecureFolderFS.Backend.Enums;
using SecureFolderFS.Backend.ViewModels.Dialogs;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

#nullable enable

namespace SecureFolderFS.WinUI.Dialogs
{
    public sealed partial class SettingsDialog : ContentDialog, IDialog<SettingsDialogViewModel>
    {
        public SettingsDialogViewModel ViewModel
        {
            get => (SettingsDialogViewModel)DataContext;
            set => DataContext = value;
        }

        public SettingsDialog()
        {
            this.InitializeComponent();
        }

        public new async Task<DialogResult> ShowAsync() => (DialogResult)await base.ShowAsync();
    }
}
