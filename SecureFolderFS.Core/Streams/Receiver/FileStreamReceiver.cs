﻿using SecureFolderFS.Core.Buffers;
using SecureFolderFS.Core.Cryptography;
using SecureFolderFS.Core.FileSystem.CryptFiles;
using SecureFolderFS.Core.FileSystem.Streams;
using System;
using System.IO;

namespace SecureFolderFS.Core.Streams.Receiver
{
    internal sealed class FileStreamReceiver : IFileStreamReceiver
    {
        private readonly ISecurity _security;
        private readonly ICryptFileManager _cryptFileManager;

        public FileStreamReceiver(ISecurity security, ICryptFileManager cryptFileManager)
        {
            _security = security;
            _cryptFileManager = cryptFileManager;
        }

        /// <inheritdoc/>
        public CleartextStream? OpenCleartextStream(string ciphertextPath, Stream ciphertextStream)
        {
            try
            {
                // Get or create encrypted file from the file system
                var openCryptFile = _cryptFileManager.TryGet(ciphertextPath)
                                    ?? _cryptFileManager.CreateNew(ciphertextPath, new HeaderBuffer(_security.HeaderCrypt.HeaderCleartextSize));

                // Check if something went wrong
                if (openCryptFile is null)
                    return null;

                // Open a new stream for that file registering existing ciphertext stream
                return openCryptFile.OpenStream(ciphertextStream);
            }
            catch (Exception)
            {
                ciphertextStream.Dispose();
                throw;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _cryptFileManager.Dispose();
        }
    }
}
