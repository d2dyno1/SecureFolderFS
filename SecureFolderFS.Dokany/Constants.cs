namespace SecureFolderFS.Dokany
{
    internal static class Constants
    {
        public static class Versioning
        {
            public const int DOKAN_VERSION = 151;

            public const int DOKAN_MAX_VERSION = 159;
        }

        public static class Options
        {
            public const uint MAX_COMPONENT_LENGTH = 256;

            public const int THREAD_COUNT = 5; // TODO: Too low?

            public const int ALLOC_UNIT_SIZE = 512;

            public const int SECTOR_SIZE = 512;

            public const DokanNet.FileAccess DATA_ACCESS =
                                                      DokanNet.FileAccess.ReadData
                                                    | DokanNet.FileAccess.WriteData
                                                    | DokanNet.FileAccess.AppendData
                                                    | DokanNet.FileAccess.Execute
                                                    | DokanNet.FileAccess.GenericExecute
                                                    | DokanNet.FileAccess.GenericWrite
                                                    | DokanNet.FileAccess.GenericRead;

            public const DokanNet.FileAccess DATA_WRITE_ACCESS =
                                                         DokanNet.FileAccess.WriteData
                                                       | DokanNet.FileAccess.AppendData
                                                       | DokanNet.FileAccess.Delete
                                                       | DokanNet.FileAccess.GenericWrite;
        }
    }
}
