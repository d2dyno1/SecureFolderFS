using SecureFolderFS.Dokany.UnsafeNative;
using SecureFolderFS.FileSystem.Adapter;
using SecureFolderFS.FileSystem.Enums;

namespace SecureFolderFS.Dokany.FileSystem
{
    public sealed class DokanyFileSystemDescriptor : IFileSystemDescriptor
    {
        public string ProviderName { get; } = "Dokany";

        public FileSystemAvailabilityErrorType IsFileSystemAvailable()
        {
            ulong dokanVersion;
            ulong dokanDriverVersion;

            try
            {
                dokanVersion = UnsafeNativeApis.DokanVersion();
                dokanDriverVersion = UnsafeNativeApis.DokanDriverVersion();
            }
            catch
            {
                return FileSystemAvailabilityErrorType.ModuleUnavailable | FileSystemAvailabilityErrorType.DriverUnavailable;
            }

            var error = FileSystemAvailabilityErrorType.None;
            error |= dokanVersion == 0 ? FileSystemAvailabilityErrorType.ModuleUnavailable : error;
            error |= dokanDriverVersion == 0 ? FileSystemAvailabilityErrorType.DriverUnavailable : error;

            if (error == FileSystemAvailabilityErrorType.None)
            {
                error |= dokanVersion >= Constants.Versioning.DOKAN_MAX_VERSION ? FileSystemAvailabilityErrorType.VersionTooHigh : error;
                error |= dokanVersion < Constants.Versioning.DOKAN_VERSION ? FileSystemAvailabilityErrorType.VersionTooLow : error;
            }

            return error == FileSystemAvailabilityErrorType.None ? FileSystemAvailabilityErrorType.FileSystemAvailable : error;
        }
    }
}
