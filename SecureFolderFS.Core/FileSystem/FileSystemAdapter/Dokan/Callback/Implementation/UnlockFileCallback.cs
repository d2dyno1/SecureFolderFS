﻿using DokanNet;
using System.IO;
using SecureFolderFS.Core.FileSystem.OpenHandles;

namespace SecureFolderFS.Core.FileSystem.FileSystemAdapter.Dokan.Callback.Implementation
{
    internal sealed class UnlockFileCallback : BaseDokanOperationsCallback, IUnlockFileCallback
    {
        public UnlockFileCallback(HandlesManager handles)
            : base(handles)
        {
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            try
            {
                if (handles.GetHandle<FileHandle>(GetContextValue(info)) is { } fileHandle)
                {
                    fileHandle.HandleStream.Unlock(offset, length);
                    return DokanResult.Success;
                }
                else
                {
                    return DokanResult.InvalidHandle;
                }
            }
            catch (IOException)
            {
                return DokanResult.AccessDenied;
            }
        }
    }
}
