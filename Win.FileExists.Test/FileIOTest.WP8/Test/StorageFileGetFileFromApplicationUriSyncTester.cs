using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFileGetFileFromApplicationUriSyncTester : IFileTester
    {
        public Task<bool> FileExistsAsync(string filename)
        {
            try
            {
                Uri appUri = new Uri("ms-appdata:///local/" + filename);
                StorageFile file = StorageFile.GetFileFromApplicationUriAsync(appUri).GetResultSync();
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