using System;

namespace SecureFolderFS.FileSystem.OpenHandles
{
    internal abstract class HandleObject : IDisposable
    {
        public abstract void Dispose();
    }
}
