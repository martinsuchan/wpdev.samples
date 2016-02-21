using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;

namespace VisibleBoundsWP8Sample
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.New) return;

            if (!VisibleBoundsExtensions.IsSupported)
            {
                MessageBox.Show(string.Format("VisibleBounds properties are not supported on WP OS: {0}", Environment.OSVersion.Version));
            }
            else
            {
                this.VisibleBoundsChangedAdd(UpdateBounds);
            }
        }

        private void ToggleAppBarClick(object sender, RoutedEventArgs e)
        {
            ApplicationBar.IsVisible = !ApplicationBar.IsVisible;
        }

        private void ToggleStatusClick(object sender, RoutedEventArgs e)
        {
            SystemTray.IsVisible = !SystemTray.IsVisible;
        }

        private void UpdateBounds(object sender, EventArgs args)
        {
            Thickness bounds = this.GetVisibleBounds();
            Top.Text = string.Format("Top {0:N1}", bounds.Top);
            Bottom.Text = string.Format("Bottom {0:N1}", bounds.Bottom);
            Left.Text = string.Format("Left {0:N1}", bounds.Left);
            Right.Text = string.Format("Right {0:N1}", bounds.Right);
        }
    }
}