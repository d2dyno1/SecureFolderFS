using System;

namespace SecureFolderFS.Core.Security.EncryptionAlgorithm
{
    public interface IAesCtrCrypt : IDisposable
    {
        Span<byte> AesCtrEncrypt(byte[] bytes, byte[] key, byte[] iv);

        Span<byte> AesCtrDecrypt(byte[] bytes, byte[] key, byte[] iv);
    }
}
