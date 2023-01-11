﻿using SecureFolderFS.Sdk.Storage.Enums;
using System.Collections.Generic;
using System.Threading;

namespace SecureFolderFS.Sdk.Storage
{
    /// <summary>
    /// Represents a folder on the file system.
    /// </summary>
    public interface IFolder : IStorable
    {
        /// <summary>
        /// Gets all items of this directory.
        /// </summary>
        /// <param name="kind">The type of items to enumerate.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> that cancels this action.</param>
        /// <returns>Returns an async operation represented by <see cref="IAsyncEnumerable{T}"/> of type <see cref="IStorable"/> of items in the directory.</returns>
        IAsyncEnumerable<IStorable> GetItemsAsync(StorableKind kind = StorableKind.All, CancellationToken cancellationToken = default);
    }
}
