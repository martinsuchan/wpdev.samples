using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Resources;
using Newtonsoft.Json;

namespace WP.DesignTime.Json
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            // to use the sample data in running app, just uncomment this
            //DataContext = new MainViewModel();
        }
    }

    // for better clarity I've stripped down all MVVM code
    public sealed class MainViewModel
    {
        public ObservableCollection<EpisodeModel> LatestEpisodes { get; set; }

        public MainViewModel()
        {
            TimelineModel timeline = GetObjectSync<TimelineModel>("timeline");
            LatestEpisodes = new ObservableCollection<EpisodeModel>(timeline.Rows);
        }

        private static T GetObjectSync<T>(string filename)
        {
            // design time data must not use async code!
            // we cannot use StorageFile classes, but old WP7 API works here
            // note the Json file must be added as 'Resource', otherwise it won't work
            Uri fileUri = new Uri($"/WP.DesignTime.Json;component/DesignTime/{filename}.json", UriKind.Relative);
            StreamResourceInfo json = Application.GetResourceStream(fileUri);
            using (StreamReader sr = new StreamReader(json.Stream))
            {
                string fileString = sr.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileString);
            }
        }
    }

    public class EpisodeModel
    {
        public string ShowName { get; set; }
        public string ImageUrl { get; set; }
        public string EpisodeName { get; set; }
    }

    public class TimelineModel
    {
        public EpisodeModel[] Rows { get; set; }
    }
}