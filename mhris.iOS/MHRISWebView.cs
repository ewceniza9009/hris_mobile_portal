using mhris.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using WebKit; // Import WebKit for WkWebViewRenderer

[assembly: ExportRenderer(typeof(WebView), typeof(MHRISWebView))]
namespace mhris.iOS
{
    public class MHRISWebView : WkWebViewRenderer
    {
        // Constructor is optional in this simple case
        public MHRISWebView() : base()
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            // JavaScript and DOM Storage are enabled by default on WKWebView.
            // No extra configuration is needed for the functionality shown
            // in the Android renderer.

            // You can add other iOS-specific customizations here if needed.
        }
    }
}