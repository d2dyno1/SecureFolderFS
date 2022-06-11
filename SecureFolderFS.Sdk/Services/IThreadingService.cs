﻿using SecureFolderFS.Shared.Utils;

namespace SecureFolderFS.Sdk.Services
{
    public interface IThreadingService
    {
        IAwaitable ExecuteOnUiThreadAsync();

        Task ExecuteOnUiThreadAsync(Action action);
    }
}