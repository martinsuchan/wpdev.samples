using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFileGetFileFromPathSyncTester : IFileTester
    {
        private readonly string rootPath = ApplicationData.Current.LocalFolder.Path;

        public Task<bool> FileExistsAsync(string filename)
        {
            try
            {
                string filePath = Path.Combine(rootPath, filename);
                StorageFile file = StorageFile.GetFileFromPathAsync(filePath).GetResultSync();
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