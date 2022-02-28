using System;

namespace SecureFolderFS.Core.Chunks.Implementation
{
    internal sealed class CiphertextAesCtrHmacChunk : BaseCiphertextChunk
    {
        public const int CHUNK_NONCE_SIZE = 16;

        public const int CHUNK_MAC_SIZE = 32;

        public const int CHUNK_FULL_CIPHERTEXT_SIZE = CHUNK_NONCE_SIZE + CleartextAesCtrHmacChunk.CHUNK_CLEARTEXT_SIZE + CHUNK_MAC_SIZE;

        public CiphertextAesCtrHmacChunk(ReadOnlyMemory<byte> ciphertextChunkBuffer)
            : base(ciphertextChunkBuffer)
        {
        }

        protected override void WithCiphertextChunkBuffer(ReadOnlyMemory<byte> ciphertextChunkBuffer)
        {
            WholeChunk = ciphertextChunkBuffer;

            Nonce = WholeChunk.Slice(0, CHUNK_NONCE_SIZE);
            Payload = WholeChunk.Slice(CHUNK_NONCE_SIZE, WholeChunk.Length - (CHUNK_NONCE_SIZE + CHUNK_MAC_SIZE));
            Auth = WholeChunk.Slice(CHUNK_NONCE_SIZE + Payload.Length, CHUNK_MAC_SIZE);
        }
    }
}
