using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFolderGetFileSyncTester : IFileTester
    {
        private readonly StorageFolder root = ApplicationData.Current.LocalFolder;

        public Task<bool> FileExistsAsync(string filename)
        {
            try
            {
                StorageFile file = root.GetFileAsync(filename).GetResultSync();
                return Task.FromResult(file != null);
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileExistsAsync error: " + e);
                return Task.FromResult(false);
            }
        }
    }
}