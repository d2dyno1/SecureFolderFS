﻿using System;

namespace SecureFolderFS.Sdk.Messages
{
    public interface IMessage
    {
    }

    public interface IMessage<T> : IMessage
    {
        T? Value { get; }
    }

    public interface IMessageWithSender : IMessage
    {
        Lazy<object?> Sender { get; }
    }
}
