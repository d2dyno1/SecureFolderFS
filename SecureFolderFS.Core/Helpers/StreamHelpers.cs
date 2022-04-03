using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using SecureFolderFS.Core.Streams;
using System.Runtime.CompilerServices;
using SecureFolderFS.Core.Streams.InternalStreams;

namespace SecureFolderFS.Core.Helpers
{
    internal static class StreamHelpers
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int ReadToIntPtrBuffer(IBaseFileStream baseFileStream, IntPtr nativeBuffer, uint bufferLength, long offset)
        {
            if (offset >= baseFileStream.Length)
            {
                return Constants.IO.FILE_EOF;
            }
            else
            {
                var readBuffer = new byte[Constants.IO.READ_BUFFER_SIZE];
                var position = 0;
                baseFileStream.Position = offset;

                do
                {
                    var remaining = (int)bufferLength - position;
                    var read = baseFileStream.Read(readBuffer, 0, Math.Min(readBuffer.Length, remaining));

                    if (read == Constants.IO.FILE_EOF)
                    {
                        return position; // Reached End-of-File EOF
                    }
                    else
                    {
                        Marshal.Copy(readBuffer, 0, nativeBuffer + position, read);
                        position += read;
                    }
                } while (position < bufferLength);

                return position;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static int WriteFromIntPtrBuffer(IBaseFileStream baseFileStream, IntPtr nativeBuffer, uint bufferLength, long offset)
        {
            var writeBuffer = new byte[Constants.IO.WRITE_BUFFER_SIZE];
            var position = 0;

            baseFileStream.Position = offset;
            do
            {
                var remaining = bufferLength - position;
                var writeLength = (int)Math.Min(remaining, writeBuffer.Length);

                Marshal.Copy(nativeBuffer + position, writeBuffer, 0, writeLength);
                baseFileStream.Write(writeBuffer, 0, writeLength);

                position += writeLength;
            } while (position < bufferLength);

            return position;
        }

        public static void WriteToStream(Stream sourceStream, Stream destinationStream)
        {
            byte[] buffer = new byte[Constants.IO.READ_BUFFER_SIZE];
            int read;

            while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, read);
            }
        }

        public static void WriteToStream(string data, Stream destinationStream, Encoding encoding = null)
        {
            using var streamWriter = new StreamWriter(destinationStream, encoding ?? Encoding.UTF8);

            streamWriter.Write(data);
        }

        public static byte[] CalculateSha1Hash(Stream stream)
        {
            using var bufferedStream = new BufferedStream(stream);
            using var sha1 = SHA1.Create();

            return sha1.ComputeHash(bufferedStream);
        }

        public static IBaseFileStreamInternal AsBaseFileStreamInternal(this IBaseFileStream baseFileStream)
        {
            return baseFileStream as IBaseFileStreamInternal;
        }

        public static ICiphertextFileStreamInternal AsCiphertextFileStreamInternal(this ICiphertextFileStream ciphertextFileStream)
        {
            return ciphertextFileStream as ICiphertextFileStreamInternal;
        }

        public static ICleartextFileStreamInternal AsCleartextFileStreamInternal(this ICleartextFileStream cleartextFileStream)
        {
            return cleartextFileStream as ICleartextFileStreamInternal;
        }

        public static long RemainingLength(this Stream stream)
        {
            return stream.Length - stream.Position;
        }

        public static bool HasRemainingLength(this Stream stream)
        {
            return stream.Position < stream.Length;
        }

        public static string ReadToEnd(this Stream stream)
        {
            using var streamReader = new StreamReader(stream, Encoding.UTF8);

            return streamReader.ReadToEnd();
        }

        public static TOutputStream Clone<TOutputStream>(this Stream stream, Func<TOutputStream> initializer = null)
            where TOutputStream : Stream, new()
        {
            var savedPosition = stream.Position;
            stream.Position = 0L;

            var clonedStream = initializer?.Invoke() ?? new TOutputStream();

            stream.SetLength(stream.Length);
            stream.CopyTo(clonedStream);
            clonedStream.Position = savedPosition;

            stream.Position = savedPosition;

            return clonedStream;
        }
    }
}
