using Android.Content;
using mhris.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(WebView), typeof(MHRISWebView))]
namespace mhris.Droid 
{
	public class MHRISWebView : WebViewRenderer
	{
		public MHRISWebView(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.Settings.JavaScriptEnabled = true;
				Control.Settings.DomStorageEnabled = true;
			}
		}
	}
}