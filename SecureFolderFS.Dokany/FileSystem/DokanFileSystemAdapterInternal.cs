using DokanNet;
using SecureFolderFS.Dokany.FileSystem.Callback;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;

namespace SecureFolderFS.Dokany.FileSystem
{
    internal sealed class DokanFileSystemAdapterInternal : IDokanOperationsUnsafe
    {
#nullable disable

        public ICleanupCallback CleanupCallback { get; init; }
        public ICloseFileCallback CloseFileCallback { get; init; }
        public ICreateFileCallback CreateFileCallback { get; init; }
        public IDeleteDirectoryCallback DeleteDirectoryCallback { get; init; }
        public IDeleteFileCallback DeleteFileCallback { get; init; }
        public IFindFilesCallback FindFilesCallback { get; init; }
        public IFindFilesWithPatternCallback FindFilesWithPatternCallback { get; init; }
        public IFindStreamsCallback FindStreamsCallback { get; init; }
        public IFlushFileBuffersCallback FlushFileBuffersCallback { get; init; }
        public IGetDiskFreeSpaceCallback GetDiskFreeSpaceCallback { get; init; }
        public IGetFileInformationCallback GetFileInformationCallback { get; init; }
        public IGetFileSecurityCallback GetFileSecurityCallback { get; init; }
        public IGetVolumeInformationCallback GetVolumeInformationCallback { get; init; }
        public ILockFileCallback LockFileCallback { get; init; }
        public IMountedCallback MountedCallback { get; init; }
        public IMoveFileCallback MoveFileCallback { get; init; }
        public IReadFileCallback ReadFileCallback { get; init; }
        public ISetAllocationSizeCallback SetAllocationSizeCallback { get; init; }
        public ISetEndOfFileCallback SetEndOfFileCallback { get; init; }
        public ISetFileAttributesCallback SetFileAttributesCallback { get; init; }
        public ISetFileSecurityCallback SetFileSecurityCallback { get; init; }
        public ISetFileTimeCallback SetFileTimeCallback { get; init; }
        public IUnlockFileCallback UnlockFileCallback { get; init; }
        public IUnmountedCallback UnmountedCallback { get; init; }
        public IWriteFileCallback WriteFileCallback { get; init; }

#nullable restore

        public void Cleanup(string fileName, IDokanFileInfo info)
        {
            CleanupCallback.Cleanup(fileName, info);
        }

        public void CloseFile(string fileName, IDokanFileInfo info)
        {
            CloseFileCallback.CloseFile(fileName, info);
        }

        public NtStatus CreateFile(string fileName, DokanNet.FileAccess access, FileShare share, FileMode mode, FileOptions options, FileAttributes attributes, IDokanFileInfo info)
        {
            return CreateFileCallback.CreateFile(fileName, access, share, mode, options, attributes, info);
        }

        public NtStatus DeleteDirectory(string fileName, IDokanFileInfo info)
        {
            return DeleteDirectoryCallback.DeleteDirectory(fileName, info);
        }

        public NtStatus DeleteFile(string fileName, IDokanFileInfo info)
        {
            return DeleteFileCallback.DeleteFile(fileName, info);
        }

        public NtStatus FindFiles(string fileName, out IList<FileInformation> files, IDokanFileInfo info)
        {
            return FindFilesCallback.FindFiles(fileName, out files, info);
        }

        public NtStatus FindFilesWithPattern(string fileName, string searchPattern, out IList<FileInformation> files, IDokanFileInfo info)
        {
            return FindFilesWithPatternCallback.FindFilesWithPattern(fileName, searchPattern, out files, info);
        }

        public NtStatus FindStreams(string fileName, out IList<FileInformation> streams, IDokanFileInfo info)
        {
            return FindStreamsCallback.FindStreams(fileName, out streams, info);
        }

        public NtStatus FlushFileBuffers(string fileName, IDokanFileInfo info)
        {
            return FlushFileBuffersCallback.FlushFileBuffers(fileName, info);
        }

        public NtStatus GetDiskFreeSpace(out long freeBytesAvailable, out long totalNumberOfBytes, out long totalNumberOfFreeBytes, IDokanFileInfo info)
        {
            return GetDiskFreeSpaceCallback.GetDiskFreeSpace(out freeBytesAvailable, out totalNumberOfBytes, out totalNumberOfFreeBytes, info);
        }

        public NtStatus GetFileInformation(string fileName, out FileInformation fileInfo, IDokanFileInfo info)
        {
            return GetFileInformationCallback.GetFileInformation(fileName, out fileInfo, info);
        }

        public NtStatus GetFileSecurity(string fileName, out FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return GetFileSecurityCallback.GetFileSecurity(fileName, out security, sections, info);
        }

        public NtStatus GetVolumeInformation(out string volumeLabel, out FileSystemFeatures features, out string fileSystemName, out uint maximumComponentLength, IDokanFileInfo info)
        {
            return GetVolumeInformationCallback.GetVolumeInformation(out volumeLabel, out features, out fileSystemName, out maximumComponentLength, info);
        }

        public NtStatus LockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return LockFileCallback.LockFile(fileName, offset, length, info);
        }

        public NtStatus Mounted(string mountPoint, IDokanFileInfo info)
        {
            return MountedCallback.Mounted(info);
        }

        public NtStatus MoveFile(string oldName, string newName, bool replace, IDokanFileInfo info)
        {
            return MoveFileCallback.MoveFile(oldName, newName, replace, info);
        }

        public NtStatus ReadFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesRead, long offset, IDokanFileInfo info)
        {
            return ReadFileCallback.ReadFile(fileName, buffer, bufferLength, out bytesRead, offset, info);
        }

        public NtStatus ReadFile(string fileName, byte[] buffer, out int bytesRead, long offset, IDokanFileInfo info)
        {
            bytesRead = 0;
            return DokanResult.NotImplemented;
        }

        public NtStatus SetAllocationSize(string fileName, long length, IDokanFileInfo info)
        {
            return SetAllocationSizeCallback.SetAllocationSize(fileName, length, info);
        }

        public NtStatus SetEndOfFile(string fileName, long length, IDokanFileInfo info)
        {
            return SetEndOfFileCallback.SetEndOfFile(fileName, length, info);
        }

        public NtStatus SetFileAttributes(string fileName, FileAttributes attributes, IDokanFileInfo info)
        {
            return SetFileAttributesCallback.SetFileAttributes(fileName, attributes, info);
        }

        public NtStatus SetFileSecurity(string fileName, FileSystemSecurity security, AccessControlSections sections, IDokanFileInfo info)
        {
            return SetFileSecurityCallback.SetFileSecurity(fileName, security, sections, info);
        }

        public NtStatus SetFileTime(string fileName, DateTime? creationTime, DateTime? lastAccessTime, DateTime? lastWriteTime, IDokanFileInfo info)
        {
            return SetFileTimeCallback.SetFileTime(fileName, creationTime, lastAccessTime, lastWriteTime, info);
        }

        public NtStatus UnlockFile(string fileName, long offset, long length, IDokanFileInfo info)
        {
            return UnlockFileCallback.UnlockFile(fileName, offset, length, info);
        }

        public NtStatus Unmounted(IDokanFileInfo info)
        {
            return UnmountedCallback.Unmounted(info);
        }

        public NtStatus WriteFile(string fileName, IntPtr buffer, uint bufferLength, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            return WriteFileCallback.WriteFile(fileName, buffer, bufferLength, out bytesWritten, offset, info);
        }

        public NtStatus WriteFile(string fileName, byte[] buffer, out int bytesWritten, long offset, IDokanFileInfo info)
        {
            bytesWritten = 0;
            return DokanResult.NotImplemented;
        }
    }
}
