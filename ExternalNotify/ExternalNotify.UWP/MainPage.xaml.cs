using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using ExternalNotify.Component;

namespace ExternalNotify
{
	public sealed partial class MainPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			Web.Navigate(new Uri("https://news.ycombinator.com/"));
		}

		private void Web_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			// WebView native object must be inserted in the OnNavigationStarting event handler
			KeyHandler winRTObject = new KeyHandler();
			// Expose the native WinRT object on the page's global object
			Web.AddWebAllowedObject("NotifyApp", winRTObject);
		}

		private async void Web_OnDOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
		{
			try
			{
				// inject event handler to arbitrary page once the DOM is loaded
				// in this case add event handlet to click on the main <table> element
				await Web.InvokeScriptAsync("eval",
					new[] { "document.getElementById(\"hnmain\").addEventListener(\"click\", function () { NotifyApp.setKeyCombination(43); }); "});
					//new[] { "document.getElementById(\"hnmain\").addEventListener(\"click\", function () { window.external.notify(\"43\"); }); " });
			}
			catch (Exception e)
			{
				Debug.WriteLine(e);
			}
		}

		private void Web_OnScriptNotify(object sender, NotifyEventArgs e)
		{
			Debug.WriteLine("Called from ScriptNotify! {0}", new[] { e.Value });
		}
	}
}