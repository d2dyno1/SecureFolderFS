﻿using System.Security.Cryptography;
using System.Text;

namespace SecureFolderFS.CLI
{
    internal static class ChecksumHelpers
    {
        public static string CalculateChecksumForPath(string path)
        {
            var buffer = Encoding.UTF8.GetBytes(path);
            var hash = MD5.HashData(buffer);

            return BitConverter.ToString(hash).Replace("-", string.Empty);
        }
    }
}
