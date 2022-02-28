using System;

namespace SecureFolderFS.Core.FileHeaders
{
    internal abstract class BaseFileHeader : IFileHeader
    {
        public ReadOnlyMemory<byte> Nonce { get; }

        public ReadOnlyMemory<byte> ContentKey { get; }

        protected BaseFileHeader(ReadOnlyMemory<byte> nonce, ReadOnlyMemory<byte> contentKey)
        {
            this.Nonce = nonce;
            this.ContentKey = contentKey;
        }
    }
}
