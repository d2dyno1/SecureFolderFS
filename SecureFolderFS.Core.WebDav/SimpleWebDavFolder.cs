﻿using SecureFolderFS.Sdk.Storage;
using SecureFolderFS.Sdk.Storage.Enums;
using SecureFolderFS.Sdk.Storage.LocatableStorage;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SecureFolderFS.Core.WebDav
{
    [Obsolete("Replace this class with NativeFolder approach once it becomes available between projects")]
    internal sealed class SimpleWebDavFolder : ILocatableFolder
    {
        public string Id { get; }
        public string Name { get; }
        public string Path { get; }

        public SimpleWebDavFolder(string path)
        {
            Id = path;
            Name = System.IO.Path.GetFileName(path);
            Path = path;
        }

        public IAsyncEnumerable<IStorable> GetItemsAsync(StorableKind kind = StorableKind.All, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ILocatableFolder?> GetParentAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
