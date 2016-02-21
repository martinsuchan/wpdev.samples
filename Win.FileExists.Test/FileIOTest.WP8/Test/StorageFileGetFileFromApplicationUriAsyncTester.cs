using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class StorageFileGetFileFromApplicationUriAsyncTester : IFileTester
    {
        public async Task<bool> FileExistsAsync(string filename)
        {
            try
            {
                Uri appUri = new Uri("ms-appdata:///local/" + filename);
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(appUri);
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