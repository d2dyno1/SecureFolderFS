using System.Runtime.CompilerServices;
using System.Text;
using SecureFolderFS.Core.Cryptography;
using SecureFolderFS.Core.FileSystem.Directories;
using SecureFolderFS.Core.FileSystem.Helpers;
using SecureFolderFS.Core.FileSystem.Paths;
using SecureFolderFS.Core.FUSE.OpenHandles;
using SecureFolderFS.Core.FUSE.UnsafeNative;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using Tmds.Fuse;
using Tmds.Linux;
using static Tmds.Linux.LibC;
using Constants = SecureFolderFS.Core.FileSystem.Constants;

namespace SecureFolderFS.Core.FUSE.Callbacks
{
    internal sealed class OnDeviceFuse : BaseFuseCallbacks
    {
        public required ILocatableFolder LocatableContentFolder { get; init; }

        public required Security Security { get; init; }

        public required IDirectoryIdAccess DirectoryIdAccess { get; init; }

        public override bool SupportsMultiThreading => true;

        public OnDeviceFuse(IPathConverter pathConverter, HandlesManager handlesManager)
            : base(pathConverter, handlesManager)
        {
        }

        // Access doesn't need to be implemented due to the default_permissions mount option

        public override unsafe int ChMod(ReadOnlySpan<byte> path, mode_t mode, FuseFileInfoRef fiRef)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            return chmod(ciphertextPathPtr, mode) == -1 ? -errno : 0;
        }

        public override unsafe int Chown(ReadOnlySpan<byte> path, uint uid, uint gid, FuseFileInfoRef fiRef)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            return chown(ciphertextPathPtr, uid, gid) == -1 ? -errno : 0;
        }

        public override unsafe int Create(ReadOnlySpan<byte> path, mode_t mode, ref FuseFileInfo fi)
        {
            var fd = creat(GetCiphertextPathPointer(path), mode);
            if (fd == -1)
                return -errno;

            close(fd);
            return Open(path, ref fi);
        }

        public override unsafe int FAllocate(ReadOnlySpan<byte> path, int mode, ulong offset, long length, ref FuseFileInfo fi)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            var handle = handlesManager.GetHandle<FuseFileHandle>(fi.fh);
            if (handle == null || !handle.FileAccess.HasFlag(FileAccess.Write))
                return -EBADF;

            var fd = open(ToUtf8ByteArray(ciphertextPath), O_WRONLY);
            if (fd == -1)
                return -errno;

            var result = fallocate(fd, mode, (long)offset, length);
            close(fd);

            return result == -1 ? -errno : 0;
        }

        public override int Flush(ReadOnlySpan<byte> path, ref FuseFileInfo fi)
        {
            var handle = handlesManager.GetHandle<FuseFileHandle>(fi.fh);
            if (handle == null || !handle.FileAccess.HasFlag(FileAccess.Write))
                return -EBADF;

            handle.Stream.Flush();
            return 0;
        }

        public override unsafe int FSync(ReadOnlySpan<byte> path, bool onlyData, ref FuseFileInfo fi)
        {
            if (handlesManager.GetHandle<FuseFileHandle>(fi.fh) == null)
                return -EBADF;

            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            var fd = open(ciphertextPathPtr, O_WRONLY);
            if (fd == -1)
                return -errno;

            var result = onlyData ? fdatasync(fd) : fsync(fd);
            close(fd);

            return result == -1 ? -errno : 0;
        }

        public override unsafe int FSyncDir(ReadOnlySpan<byte> path, bool onlyData, ref FuseFileInfo fi)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            var fd = UnsafeNativeApis.OpenDir(ciphertextPathPtr);
            if (fd == -1)
                return -errno;

            var result = onlyData ? fdatasync(fd) : fsync(fd);
            close(fd);

            return result == 0 ? 0 : -errno;
        }

        public override unsafe int GetAttr(ReadOnlySpan<byte> path, ref stat stat, FuseFileInfoRef fiRef)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            if (!path.SequenceEqual(RootPath) && !File.Exists(ciphertextPath) && !Directory.Exists(ciphertextPath))
                return -ENOENT;

            fixed (stat *statPtr = &stat)
            {
                var result = LibC.stat(ToUtf8ByteArray(ciphertextPath), statPtr);
                if (File.Exists(ciphertextPath))
                    stat.st_size = Math.Max(0, Security.ContentCrypt.CalculateCleartextSize(stat.st_size - Security.HeaderCrypt.HeaderCiphertextSize));

                return result == -1 ? -errno : 0;
            }
        }

        public override unsafe int GetXAttr(ReadOnlySpan<byte> path, ReadOnlySpan<byte> name, Span<byte> value)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            fixed (byte *namePtr = name)
            fixed (void *valuePtr = value)
            {
                var result = UnsafeNativeApis.GetXAttr(ciphertextPathPtr, namePtr, valuePtr, value.Length);
                return result == -1 ? -errno : result;
            }
        }

        public override unsafe int ListXAttr(ReadOnlySpan<byte> path, Span<byte> list)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            fixed (byte *listPtr = list)
            {
                var result = UnsafeNativeApis.ListXAttr(ciphertextPathPtr, listPtr, list.Length);
                return result == -1 ? -errno : result;
            }
        }

        public override unsafe int MkDir(ReadOnlySpan<byte> path, mode_t mode)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            if (mkdir(ToUtf8ByteArray(ciphertextPath), mode) == -1)
                return -errno;

                // Initialize directory with directory ID
            var directoryIdPath = Path.Combine(ciphertextPath, Constants.DIRECTORY_ID_FILENAME);
            _ = DirectoryIdAccess.SetDirectoryId(directoryIdPath, DirectoryId.CreateNew());

            return 0;
        }

        public override int Open(ReadOnlySpan<byte> path, ref FuseFileInfo fi)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            var mode = FileMode.Open;
            if ((fi.flags & O_APPEND) != 0)
                mode = FileMode.Append;
            else if ((fi.flags & O_CREAT) != 0)
            {
                // TODO this breaks LibreOffice's lock files
                // if ((fi.flags & O_EXCL) != 0 && File.Exists(ciphertextPath))
                // return -EEXIST;

                // TODO Don't follow symlinks if O_EXCL is specified
                mode = FileMode.Create;
            }
            else if ((fi.flags & O_TRUNC) != 0)
                mode = FileMode.Truncate;

            // TODO Check if path is a symlink
            // if ((fi.flags & O_NOFOLLOW) != 0)
            // return -ELOOP;

            // Files are sometimes opened without an access flag, even though it should always be present.
            // Also the stream can't be write only for some reason.
            var access = (fi.flags & O_RDONLY) != 0 ? FileAccess.Read : FileAccess.ReadWrite;

            var options = FileOptions.None;
            if ((fi.flags & O_ASYNC) != 0)
                options |= FileOptions.Asynchronous;
            if ((fi.flags & O_DIRECT) != 0)
            {
                options |= FileOptions.WriteThrough;
                fi.direct_io = true;
            }
            if ((fi.flags & O_TMPFILE) != 0)
                options |= FileOptions.DeleteOnClose;

            var handle = handlesManager.OpenHandleToFile(ciphertextPath, mode, access, FileShare.ReadWrite, options);
            if (handle == null)
                return -EACCES;

            fi.fh = handle.Value;
            return 0;
        }

        public override int OpenDir(ReadOnlySpan<byte> path, ref FuseFileInfo fi)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null || !Directory.Exists(ciphertextPath))
                return -ENOENT;

            if (File.Exists(ciphertextPath))
                return -ENOTDIR;

            return 0;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int Read(ReadOnlySpan<byte> path, ulong offset, Span<byte> buffer, ref FuseFileInfo fi)
        {
            var handle = handlesManager.GetHandle<FuseFileHandle>(fi.fh);
            if (handle == null)
                return -EBADF;

            handle.Stream.Position = (long)offset;
            return handle.Stream.Read(buffer);
        }

        public override int ReadDir(ReadOnlySpan<byte> path, ulong offset, ReadDirFlags flags, DirectoryContent content, ref FuseFileInfo fi)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            content.AddEntry(".");
            content.AddEntry("..");

            foreach (var file in Directory.EnumerateFiles(ciphertextPath))
            {
                if (Path.GetFileName(file) != Constants.DIRECTORY_ID_FILENAME)
                    content.AddEntry(pathConverter.GetCleartextFileName(file));
            }

            foreach (var directory in Directory.EnumerateDirectories(ciphertextPath))
                content.AddEntry(pathConverter.GetCleartextFileName(directory));

            return 0;
        }

        public override void Release(ReadOnlySpan<byte> path, ref FuseFileInfo fi)
        {
            handlesManager.CloseHandle(fi.fh);
        }

        public override int ReleaseDir(ReadOnlySpan<byte> path, ref FuseFileInfo fi)
        {
            return 0;
        }

        public override unsafe int RemoveXAttr(ReadOnlySpan<byte> path, ReadOnlySpan<byte> name)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            fixed (byte *namePtr = name)
                return UnsafeNativeApis.RemoveXAttr(ciphertextPathPtr, namePtr) == -1 ? -errno : 0;
        }

        public override unsafe int Rename(ReadOnlySpan<byte> path, ReadOnlySpan<byte> newPath, int flags)
        {
            return UnsafeNativeApis.RenameAt2(0, GetCiphertextPathPointer(path), 0, GetCiphertextPathPointer(newPath), (uint)flags) == -1 ? errno : 0;
        }

        public override unsafe int RmDir(ReadOnlySpan<byte> path)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            if (Directory.EnumerateFileSystemEntries(ciphertextPath).Any(x => Path.GetFileName(x) != Constants.DIRECTORY_ID_FILENAME))
                return -ENOTEMPTY;

            DirectoryIdAccess.RemoveDirectoryId(ciphertextPath);
            File.Delete(Path.Combine(ciphertextPath, Constants.DIRECTORY_ID_FILENAME));

            return rmdir(ToUtf8ByteArray(ciphertextPath)) == -1 ? -errno : 0;
        }

        public override unsafe int SetXAttr(ReadOnlySpan<byte> path, ReadOnlySpan<byte> name, ReadOnlySpan<byte> value, int flags)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            fixed (byte *namePtr = name)
            fixed (void *valuePtr = value)
                return UnsafeNativeApis.SetXAttr(ciphertextPathPtr, namePtr, valuePtr, value.Length, flags) == -1 ? -errno : 0;
        }

        public override unsafe int StatFS(ReadOnlySpan<byte> path, ref statvfs statfs)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            fixed (statvfs *statfsPtr = &statfs)
                return statvfs(ciphertextPathPtr, statfsPtr) == -1 ? -errno : 0;
        }

        public override unsafe int Truncate(ReadOnlySpan<byte> path, ulong length, FuseFileInfoRef fiRef)
        {
            if (!fiRef.IsNull)
            {
                var handle = handlesManager.GetHandle<FuseFileHandle>(fiRef.Value.fh);
                if (handle != null && !handle.FileAccess.HasFlag(FileAccess.Write))
                    return -EBADF;
            }

            var ciphertextPath = GetCiphertextPath(path);
            if (ciphertextPath == null)
                return -ENOENT;

            return truncate(ToUtf8ByteArray(ciphertextPath), (long)length) == -1 ? -errno : 0;
        }

        /// <remarks>
        /// This method is also responsible for file deletion.
        /// </remarks>
        public override unsafe int Unlink(ReadOnlySpan<byte> path)
        {
            var ciphertextPath = GetCiphertextPath(path);
            if (!File.Exists(ciphertextPath))
                return -ENOENT;
            if (Directory.Exists(ciphertextPath))
                return -EISDIR;

            return unlink(ToUtf8ByteArray(ciphertextPath)) == -1 ? -errno : 0;
        }

        public override unsafe int UpdateTimestamps(ReadOnlySpan<byte> path, ref timespec atime, ref timespec mtime, FuseFileInfoRef fiRef)
        {
            var ciphertextPathPtr = GetCiphertextPathPointer(path);
            if (ciphertextPathPtr == null)
                return -ENOENT;

            if (!fiRef.IsNull && handlesManager.GetHandle<FuseFileHandle>(fiRef.Value.fh) == null)
                return -EBADF;

            var fd = open(ciphertextPathPtr, O_WRONLY);
            if (fd == -1)
                return -errno;

            fixed (timespec *times = new[] { atime, mtime })
            {
                var result = futimens(fd, times);
                close(fd);

                return result == -1 ? -errno : 0;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override int Write(ReadOnlySpan<byte> path, ulong offset, ReadOnlySpan<byte> buffer, ref FuseFileInfo fi)
        {
            var handle = handlesManager.GetHandle<FuseFileHandle>(fi.fh);
            if (handle == null)
                return -EBADF;

            handle.Stream.Position = (long)offset;
            handle.Stream.Write(buffer);

            return buffer.Length;
        }

        protected override unsafe string? GetCiphertextPath(ReadOnlySpan<byte> cleartextName)
        {
            fixed (byte *cleartextNamePtr = cleartextName)
            {
                var path = PathHelpers.PathFromVaultRoot(Encoding.UTF8.GetString(cleartextNamePtr, cleartextName.Length), LocatableContentFolder.Path);
                return pathConverter.ToCiphertext(path);
            }
        }
    }
}