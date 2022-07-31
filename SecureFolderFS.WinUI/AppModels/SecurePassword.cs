﻿using System;
using SecureFolderFS.Shared.Utils;

namespace SecureFolderFS.WinUI.AppModels
{
    internal sealed class SecurePassword : IPassword
    {
        private readonly byte[] _password;

        public SecurePassword(byte[] password)
        {
            _password = password;
        }

        /// <inheritdoc/>
        public byte[] GetPassword()
        {
            return _password;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Array.Clear(_password);
        }
    }
}
