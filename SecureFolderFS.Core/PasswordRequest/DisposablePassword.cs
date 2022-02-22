﻿using SecureFolderFS.Core.SecureStore;
using System;

#nullable enable

namespace SecureFolderFS.Core.PasswordRequest
{
    /// <summary>
    /// Provides implementation for password with on-demand <see cref="IDisposable"/> disposing.
    /// <br/>
    /// <br/>
    /// This SDK is exposed.
    /// </summary>
    public sealed class DisposablePassword : IDisposable
    {
        internal DisposableArray Password { get; }

        public int Length => Password.Bytes.Length;

        public DisposablePassword(byte[] password)
        {
            this.Password = new(password);
        }

        public static DisposablePassword AsEmpty()
        {
            return new DisposablePassword(new byte[0]);
        }

        public void Dispose()
        {
            Password.Dispose();
        }
    }
}
