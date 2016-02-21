using System;
using System.Diagnostics;
using Windows.UI.Xaml.Navigation;
using FileIOTest.Test;

namespace FileIOTest.Win8
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            try
            {
                TestRunner runner = new TestRunner();
                Block.DataContext = runner;

                IFileTester[] testers =
                {
#if WINDOWS_APP
                    //new StorageFolderTryGetItemAsyncTester(),
                    new StorageFolderTryGetItemSyncTester(),
#endif
                    /*new StorageFolderGetFileAsyncTester(),
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