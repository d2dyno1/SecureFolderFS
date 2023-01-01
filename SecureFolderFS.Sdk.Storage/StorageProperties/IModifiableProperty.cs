﻿using System.Threading.Tasks;

namespace SecureFolderFS.Sdk.Storage.StorageProperties
{
    /// <summary>
    /// Represents a storage object property that can be modified.
    /// </summary>
    public interface IModifiableProperty<T> : IStorageProperty<T>
    {
        /// <summary>
        /// Updates the value of the property notifying all handlers.
        /// </summary>
        /// <param name="newValue">The new value to set.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task ModifyAsync(T newValue);
    }
}
