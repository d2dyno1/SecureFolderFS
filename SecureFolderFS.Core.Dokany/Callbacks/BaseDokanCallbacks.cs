﻿using DokanNet;
using SecureFolderFS.Core.Dokany.OpenHandles;
using SecureFolderFS.Core.FileSystem.Helpers;
using SecureFolderFS.Core.FileSystem.Paths;
using System.Runtime.CompilerServices;

namespace SecureFolderFS.Core.Dokany.Callbacks
{
    internal abstract class BaseDokanCallbacks
    {
        protected readonly string vaultRootPath;
        protected readonly IPathConverter pathConverter;
        protected readonly HandlesManager handlesManager;

        protected BaseDokanCallbacks(string vaultRootPath, IPathConverter pathConverter, HandlesManager handlesManager)
        {
            this.vaultRootPath = vaultRootPath;
            this.pathConverter = pathConverter;
            this.handlesManager = handlesManager;
        }

        // TODO: Add checks for nullable in places where this function is called
        protected string? GetCiphertextPath(string cleartextName)
        {
            var path = PathHelpers.PathFromVaultRoot(cleartextName, vaultRootPath);
            return pathConverter.ToCiphertext(path);
        }

        protected void CloseHandle(IDokanFileInfo info)
        {
            handlesManager.CloseHandle(GetContextValue(info));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static bool IsContextInvalid(IDokanFileInfo info)
        {
            return GetContextValue(info) == Constants.FileSystem.INVALID_HANDLE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static void InvalidateContext(IDokanFileInfo info)
        {
            info.Context = Constants.FileSystem.INVALID_HANDLE;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static long GetContextValue(IDokanFileInfo info)
        {
            return info.Context is long ctxLong ? ctxLong : Constants.FileSystem.INVALID_HANDLE;
        }
    }
}