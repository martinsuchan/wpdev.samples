using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;

namespace FileIOTest.Test
{
    public class TestRunner : INotifyPropertyChanged
    {
        public string Log
        {
            get { return log; }
            set { Set(ref log, value); }
        }
        private string log;

        private int filesToTest;
        private double createRatio;
        private readonly Dictionary<string, bool> fileMap = new Dictionary<string, bool>();

        public async Task InitFiles(int filesToTest, double createRatio)
        {
            this.filesToTest = filesToTest;
            this.createRatio = createRatio;

            await AppendLog(string.Format("Initializing test with {0} files, exists ratio {1}", this.filesToTest, this.createRatio));
            int filesCreated = 0;
            fileMap.Clear();
            StorageFolder root = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> files = await root.GetFilesAsync();
            for (int i = 0; i < this.filesToTest; i++)
            {
                string fileName = i.ToString();
                if (filesCreated < (i*this.createRatio))
                {
                    await root.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                    filesCreated++;
                    fileMap[fileName] = true;
                }
                else
                {
                    StorageFile file = files.FirstOrDefault(f => f.Name == i.ToString());
                    if (file != null)
                    {
                        await file.DeleteAsync();
                    }
                    fileMap[fileName] = false;
                }
            }
        }

        public async Task RunTest(IFileTester[] testers)
        {
            await AppendLog("Starting test");
            foreach (IFileTester tester in testers)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                foreach (KeyValuePair<string, bool> pair in fileMap)
                {
                    bool exists = await tester.FileExistsAsync(pair.Key);
                    if (exists != pair.Value)
                    {
                        await AppendLog(string.Format("Error in tester {0}", tester.GetType().FullName));
                        return;
                    }
                }

                sw.Stop();
                await AppendLog(string.Format("{0}: {1:N0} ms", tester.GetType().Name, sw.ElapsedMilliseconds));
            }
            await AppendLog(string.Empty);
        }

        private async Task AppendLog(string message)
        {
            Log = Log + message + Environment.NewLine;
            await Task.Yield();
        }

        #region INotifyPropertyChanged implementation

        private const string FakePropertyName = @"A property name must be specified if not using C# 5/VS2012";

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool Set<TName>(ref TName field, TName newValue,
            [CallerMemberName] string propertyName = FakePropertyName)
        {
            if (EqualityComparer<TName>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = FakePropertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}