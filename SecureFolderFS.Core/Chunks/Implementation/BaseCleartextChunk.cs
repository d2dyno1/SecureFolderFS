using System;
using System.IO;
using SecureFolderFS.Core.Extensions;

namespace SecureFolderFS.Core.Chunks.Implementation
{
    internal abstract class BaseCleartextChunk : ICleartextChunk
    {
        protected Memory<byte> Buffer { get; }

        public bool NeedsFlush { get; private set; }

        public int ActualLength { get; private set; }

        protected BaseCleartextChunk(Memory<byte> cleartextChunkBuffer, int actualLength)
        {
            this.Buffer = cleartextChunkBuffer;
            this.ActualLength = actualLength;
        }

        public virtual void CopyTo(MemoryStream destinationStream, int offset)
        {
            var writeCount = Math.Min(ActualLength - offset, (int)destinationStream.RemainingLength());
            destinationStream.Write(Buffer.Span.Slice(offset, writeCount));
        }

        public virtual void CopyFrom(MemoryStream sourceStream, int offset)
        {
            NeedsFlush = true;

            var readCount = Math.Min(Buffer.Length - offset, (int)sourceStream.RemainingLength());
            sourceStream.Read(Buffer.Span.Slice(offset, readCount));
            ActualLength = Math.Max(ActualLength, readCount + offset);
        }

        public virtual void SetActualLength(int length)
        {
            if (ActualLength > length)
            {
                ActualLength = length;
                NeedsFlush = true;
            }
        }

        public virtual ReadOnlySpan<byte> AsSpan()
        {
            return Buffer.Span;
        }
    }
}
