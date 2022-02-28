using System;

namespace SecureFolderFS.Core.Chunks.Implementation
{
    internal sealed class CiphertextXChaCha20Chunk : BaseCiphertextChunk
    {
        public const int CHUNK_NONCE_SIZE = 24;

        public const int CHUNK_TAG_SIZE = 16;

        public const int CHUNK_FULL_CIPHERTEXT_SIZE = CHUNK_NONCE_SIZE + CleartextXChaCha20Chunk.CHUNK_CLEARTEXT_SIZE + CHUNK_TAG_SIZE;

        public CiphertextXChaCha20Chunk(ReadOnlyMemory<byte> ciphertextChunkBuffer)
            : base(ciphertextChunkBuffer)
        {
        }

        protected override void WithCiphertextChunkBuffer(ReadOnlyMemory<byte> ciphertextChunkBuffer)
        {
            WholeChunk = ciphertextChunkBuffer;

            Nonce = WholeChunk.Slice(0, CHUNK_NONCE_SIZE);
            Payload = WholeChunk.Slice(CHUNK_NONCE_SIZE, WholeChunk.Length - (CHUNK_NONCE_SIZE + CHUNK_TAG_SIZE));
            Auth = WholeChunk.Slice(CHUNK_NONCE_SIZE + Payload.Length, CHUNK_TAG_SIZE);
        }
    }
}
