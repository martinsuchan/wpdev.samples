using System.Diagnostics;
using Windows.Foundation.Metadata;

namespace ExternalNotify.Component
{
	public delegate void NotifyAppHandler(int keyComb);

	/// <summary>
	/// Sample native object for injecting to the WebView.
	/// </summary>
	[AllowForWeb]
	public sealed class KeyHandler
	{
		public event NotifyAppHandler NotifyAppEvent;

		public void setKeyCombination(int keyPress)
		{
			Debug.WriteLine("Called from WebView! {0}", keyPress);
		}
	}
}