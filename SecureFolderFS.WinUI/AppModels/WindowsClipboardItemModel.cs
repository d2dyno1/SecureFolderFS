﻿using SecureFolderFS.Sdk.Enums;
using SecureFolderFS.Sdk.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace SecureFolderFS.WinUI.AppModels
{
    /// <inheritdoc cref="IClipboardItemModel"/>
    internal sealed class WindowsClipboardItemModel : IClipboardItemModel
    {
        private readonly DataPackageView _dataPackageView;

        /// <inheritdoc/>
        public ClipboardDataType DataType { get; }

        public WindowsClipboardItemModel(DataPackageView dataPackageView)
        {
            _dataPackageView = dataPackageView;
            DataType = GetDataType(dataPackageView);
        }

        /// <inheritdoc/>
        public async Task<object?> GetDataAsync()
        {
            return DataType switch
            {
                ClipboardDataType.Text => await _dataPackageView.GetTextAsync(),
                _ => null
            };
        }

        private static ClipboardDataType GetDataType(DataPackageView dataPackageView)
        {
            if (dataPackageView.AvailableFormats.Contains(StandardDataFormats.Text))
            {
                return ClipboardDataType.Text;
            }

            return ClipboardDataType.Unknown;
        }
    }
}
