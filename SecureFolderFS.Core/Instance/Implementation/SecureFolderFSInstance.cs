﻿using SecureFolderFS.Core.Exceptions;
using SecureFolderFS.Core.FileSystem.Analytics;
using SecureFolderFS.Core.FileSystem.Paths;
using SecureFolderFS.Core.Helpers;
using SecureFolderFS.Core.Streams.Receiver;

namespace SecureFolderFS.Core.Instance.Implementation
{
    internal sealed class SecureFolderFSInstance : ISecureFolderFSInstance
    {
        public string MountLocation { get; internal set; }

        public IPathConverter PathConverter { get; internal set; }

        internal IFileSystemStatsTracker FileSystemStatsTracker { get; set; }

        internal IFileStreamReceiver FileStreamReceiver { get; set; }

        internal IFileSystemAdapter FileSystemAdapter { get; set; }

        internal IFileSystemOperations FileSystemOperations { get; set; }

        public void StartFileSystem()
        {
            if (MountLocation is null)
            {
                var mountLetter = VaultHelpers.GetUnusedMountLetter();
                if (mountLetter == char.MinValue)
                {
                    throw new VaultInitializationException("No free mount point exists to mount the vault.");
                }

                MountLocation = $"{mountLetter}:\\";
            }

            FileSystemAdapter.StartFileSystem(MountLocation);
        }

        public void Dispose()
        {
            FileSystemAdapter?.Dispose();
            FileSystemStatsTracker?.Dispose();
            FileStreamReceiver?.Dispose();
            FileSystemAdapter?.Dispose();
        }
    }
}
