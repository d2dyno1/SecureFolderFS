﻿using System;
using System.Threading.Tasks;

namespace SecureFolderFS.WinUI.Dispatching
{
    /// <summary>
    /// Interface that represents a thread dispatcher. 
    /// </summary>
    public interface IThreadDispatcher
    {
        bool HasThreadAccess { get; }

        /// <summary>
        /// Dispatches an action to run on different thread.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <returns></returns>
        Task DispatchAsync(Action action);
    }
}