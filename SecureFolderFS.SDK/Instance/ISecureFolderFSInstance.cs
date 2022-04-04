using System;
using SecureFolderFS.SDK.Paths;

namespace SecureFolderFS.SDK.Instance
{
    public interface ISecureFolderFSInstance : IDisposable
    {
        MountVolumeDataModel
        
        string MountLocation { get; }

        IPathReceiver PathReceiver { get; }

        void StartFileSystem();

        void StopFileSystem();
    }
}
