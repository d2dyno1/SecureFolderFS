using DokanNet;
using SecureFolderFS.FileSystem.Adapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Dokany.FileSystem
{
    internal sealed class DokanyFileSystemAdapter : IFileSystemAdapter
    {
        private readonly DokanFileSystemAdapterInternal _fileSystemIntrinsics;

        private DokanInstance? _dokanInstance;

        public DokanyFileSystemAdapter(DokanFileSystemAdapterInternal fileSystemIntrinsics)
        {
            _fileSystemIntrinsics = fileSystemIntrinsics;
        }

        public void InitializeFileSystem()
        {
            Dokan.Init();
        }

        public void StartFileSystem(string mountLocation)
        {
            var options = DokanOptions.CurrentSession | DokanOptions.CaseSensitive | DokanOptions.FixedDrive;

            _dokanInstance = _fileSystemIntrinsics.CreateFileSystem(
                mountLocation,
                options,
                false,
                200,
                TimeSpan.FromSeconds(20),
                SecureFolderFS.FileSystem.Constants.FileSystem.UNC_NAME,
                512,
                512);
        }

        public bool StopFileSystem(string mountLocation)
        {
            _dokanInstance?.Dispose();

            return _dokanInstance != null;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
