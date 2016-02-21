using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class FileExistsTester : IFileTester
    {
        private readonly string rootPath = ApplicationData.Current.LocalFolder.Path;

        public Task<bool> FileExistsAsync(string filename)
        {
            string filePath = Path.Combine(rootPath, filename);
            return Task.FromResult(File.Exists(filePath));
        }
    }
}