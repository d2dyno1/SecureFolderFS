using System;

namespace SecureFolderFS.Core.Chunks.Implementation
{
    internal abstract class BaseCiphertextChunk : ICiphertextChunk
    {
        protected ReadOnlyMemory<byte> WholeChunk { get; set; }

        public ReadOnlyMemory<byte> Nonce { get; protected set; }

        public ReadOnlyMemory<byte> Payload { get; protected set; }

        public ReadOnlyMemory<byte> Auth { get; protected set; }

        protected BaseCiphertextChunk(ReadOnlyMemory<byte> ciphertextChunkBuffer)
        {
            WithCiphertextChunkBuffer(ciphertextChunkBuffer);
        }

        public virtual ReadOnlyMemory<byte> ToArray()
        {
            return WholeChunk;
        }

        protected abstract void WithCiphertextChunkBuffer(ReadOnlyMemory<byte> ciphertextChunkBuffer);
    }
}
