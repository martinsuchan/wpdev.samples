using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFolderTryGetItemSyncTester : IFileTester
    {
        private readonly StorageFolder root = ApplicationData.Current.LocalFolder;

        public Task<bool> FileExistsAsync(string filename)
        {
            StorageFile file = root.TryGetItemAsync(filename).GetResultSync() as StorageFile;
            return Task.FromResult(file != null);
        }
    }
}