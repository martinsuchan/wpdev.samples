using System;
using System.Diagnostics;
using System.Windows.Navigation;
using FileIOTest.Test;

namespace FileIOTest
{
    public partial class MainPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                TestRunner runner = new TestRunner();
                Block.DataContext = runner;

                IFileTester[] testers =
                {
                    /*new FileExistsTester(),
                    new IsolatedStorageFileFileExistsTester(),
                    new StorageFolderGetFileAsyncTester(),
                    new StorageFileGetFileFromPathAsyncTester(),
                    new StorageFileGetFileFromApplicationUriAsyncTester(),
                    new StorageFolderGetFilesAsyncTester(),*/
                    new StorageFolderGetFileSyncTester(),
                    new StorageFileGetFileFromPathSyncTester(),
                    new StorageFileGetFileFromApplicationUriSyncTester(),
                };

                await runner.InitFiles(500, 0);
                await runner.RunTest(testers);
                await runner.InitFiles(500, 0.5);
                await runner.RunTest(testers);
                await runner.InitFiles(500, 1);
                await runner.RunTest(testers);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}