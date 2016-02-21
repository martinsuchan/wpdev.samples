using System.IO.IsolatedStorage;
using System.Threading.Tasks;

namespace FileIOTest.Test
{
    public class IsolatedStorageFileFileExistsTester : IFileTester
    {
        private readonly IsolatedStorageFile root = IsolatedStorageFile.GetUserStoreForApplication();

        public Task<bool> FileExistsAsync(string filename)
        {
            return Task.FromResult(root.FileExists(filename));
        }
    }
}