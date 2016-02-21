using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFileGetFileFromPathAsyncTester : IFileTester
    {
        private readonly string rootPath = ApplicationData.Current.LocalFolder.Path;

        public async Task<bool> FileExistsAsync(string filename)
        {
            try
            {
                string filePath = Path.Combine(rootPath, filename);
                StorageFile file = await StorageFile.GetFileFromPathAsync(filePath);
                return file != null;
            }
            catch (Exception e)
            {
                Debug.WriteLine("FileExistsAsync error: " + e);
                return false;
            }
        }
    }
}