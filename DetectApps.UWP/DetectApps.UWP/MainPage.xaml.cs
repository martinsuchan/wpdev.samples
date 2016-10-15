using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Foundation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace DetectApps.UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage
    {
        public AppStatus[] Apps { get; }

        public MainPage()
        {
            Apps = new[]
            {
                new AppStatus("VLC", "VideoLAN.VLC_paz6r1rewnh0a"),
                new AppStatus("Facebook", "Facebook.Facebook_8xx8rvfyw5nnt"),
                new AppStatus("Twitter", "9E2F88E3.Twitter_wgeqdkkx372wm"),
                new AppStatus("AdBlock - Edge extension", "BetaFish.AdBlock_c1wakc4j0nefm"),
                new AppStatus("Astronomy Picture of the Day", "54490MartinSuchan.APOD_aabn1bapetf12"),
                new AppStatus("NASA TV Live", "54490MartinSuchan.NASATVLive_aabn1bapetf12"),
                new AppStatus("Microsoft Mahjong", "Microsoft.MicrosoftMahjong_8wekyb3d8bbwe"),
                new AppStatus("Slither Snake.io", "56081SweetGamesBox.SlitherSnake.io_v5wzgnqbvrv1e"),
                new AppStatus("Despicable Me: Minion Rush - Win8.1 app", "GAMELOFTSA.DespicableMeMinionRush_0pp20fcewvvtj"),
                new AppStatus("Hill Climb Racing", "Fingersoft.HillClimbRacing_r6rtpscs7gwyg"),
            };

            InitializeComponent();
        }

	    protected override void OnNavigatedTo(NavigationEventArgs e)
	    {
		    base.OnNavigatedTo(e);
		    ApplicationView.GetForCurrentView().TryResizeView(new Size(500, 600));
	    }

	    private async void Button_OnClick(object sender, RoutedEventArgs e)
	    {
		    foreach (var app in Apps)
		    {
			    bool isInstalled = await StoreHelper.IsAppInstalledAsync(app.AppPackageName);
			    app.InstallStatus = isInstalled ? "App is installed" : "App is NOT installed";
		    }
	    }
    }

    public class AppStatus : INotifyPropertyChanged
    {
        public string AppTitle { get; set; }
        public string AppPackageName { get; set; }

        public string InstallStatus
        {
            get { return installStatus; }
            set { Set(ref installStatus, value); }
        }
        private string installStatus;

        public AppStatus(string appTitle, string appPackageName)
        {
            AppTitle = appTitle;
            AppPackageName = appPackageName;
        }

        #region INotifyPropertyChanged implementation

        public event PropertyChangedEventHandler PropertyChanged;

        private bool Set<TName>(ref TName field, TName newValue, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<TName>.Default.Equals(field, newValue))
            {
                return false;
            }

            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}