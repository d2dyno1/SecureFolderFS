﻿namespace SecureFolderFS.Sdk.Services
{
    public interface IFileExplorerService
    {
        Task OpenAppFolderAsync();

        Task OpenPathInFileExplorerAsync(string path);

        Task<string?> PickSingleFileAsync(IEnumerable<string>? filter);

        Task<string?> PickSingleFolderAsync();
    }
}
