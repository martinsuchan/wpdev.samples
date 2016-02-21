using System.Threading.Tasks;

namespace FileIOTest.Test
{
    public interface IFileTester
    {
        Task<bool> FileExistsAsync(string filename);
    }
}