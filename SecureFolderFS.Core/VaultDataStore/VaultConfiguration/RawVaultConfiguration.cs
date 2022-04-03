using Newtonsoft.Json;
using System.IO;
using SecureFolderFS.Shared.Extensions;
using SecureFolderFS.Core.Helpers;

namespace SecureFolderFS.Core.VaultDataStore.VaultConfiguration
{
    public sealed class RawVaultConfiguration : VaultVersion
    {
        public readonly string rawData;

        private RawVaultConfiguration(string rawData, int version)
            : base(version)
        {
            this.rawData = rawData;
        }

        public static RawVaultConfiguration Load(Stream configFileStream)
        {
            // Get data from the config file
            var rawData = configFileStream.ReadToEnd();

            // Get vault version
            var vaultVersion = JsonConvert.DeserializeObject<VaultVersion>(rawData); // TODO: Use json validator

            return new RawVaultConfiguration(rawData, vaultVersion);
        }
    }
}
