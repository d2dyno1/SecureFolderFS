﻿using System;

namespace SecureFolderFS.Core.Security.EncryptionAlgorithm
{
    public interface IAesSivCrypt
    {
        void Encrypt(ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> encryptionKey, ReadOnlySpan<byte> macKey, ReadOnlySpan<byte> associatedData, Span<byte> result);

        bool Decrypt(ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> encryptionKey, ReadOnlySpan<byte> macKey, ReadOnlySpan<byte> associatedData, Span<byte> result);
    }
}
