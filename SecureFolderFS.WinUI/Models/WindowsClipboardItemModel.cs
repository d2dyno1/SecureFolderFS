﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using SecureFolderFS.Sdk.Enums;
using SecureFolderFS.Sdk.Models;

namespace SecureFolderFS.WinUI.Models
{
    /// <inheritdoc cref="IClipboardDataModel"/>
    internal sealed class WindowsClipboardItemModel : IClipboardDataModel
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
            switch (DataType)
            {
                case ClipboardDataType.Text:
                    return await _dataPackageView.GetTextAsync();
            }

            return null;
        }

        private ClipboardDataType GetDataType(DataPackageView dataPackageView)
        {
            if (dataPackageView.AvailableFormats.Contains(StandardDataFormats.Text))
            {
                return ClipboardDataType.Text;
            }

            return ClipboardDataType.Unknown;
        }
    }
}
