using System.Threading;

namespace SecureFolderFS.FileSystem.OpenHandles
{
    internal sealed class HandleGenerator
    {
        private long _handleCounter;

        public long ThreadSafeIncrementAndGet()
        {
            Interlocked.Increment(ref _handleCounter);
            return _handleCounter;
        }
    }
}
