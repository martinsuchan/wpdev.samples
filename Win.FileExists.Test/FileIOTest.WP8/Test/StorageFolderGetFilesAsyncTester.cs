using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFolderGetFilesAsyncTester : IFileTester
    {
        private readonly StorageFolder root = ApplicationData.Current.LocalFolder;

        public async Task<bool> FileExistsAsync(string filename)
        {
            IReadOnlyList<StorageFile> files = await root.GetFilesAsync();
            return files.Any(f => f.Name == filename);
        }
    }
}