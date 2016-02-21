using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class ManualHashsetTester : IFileTester
    {
        private readonly StorageFolder root = ApplicationData.Current.LocalFolder;
        private readonly HashSet<string> hashSet = new HashSet<string>();
        private readonly SemaphoreSlim ss = new SemaphoreSlim(1);

        private ManualHashsetTester() {}

        public static async Task<ManualHashsetTester> Get()
        {
            ManualHashsetTester tester = new ManualHashsetTester();
            await tester.ss.WaitAsync();

            IReadOnlyList<StorageFile> files = await tester.root.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                tester.hashSet.Add(file.Name);
            }
            tester.ss.Release();
            return tester;
        }

        public async Task<bool> FileExistsAsync(string filename)
        {
            await ss.WaitAsync();
            bool exists = hashSet.Contains(filename);
            ss.Release();
            return exists;
        }
    }
}