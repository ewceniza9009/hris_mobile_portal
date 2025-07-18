using System;
using Foundation;
using UIKit;

namespace mhris.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            // Note: Xamarin.Essentials and CurrentActivityPlugin do not require explicit
            // initialization on iOS in the same way as Android. They are generally ready
            // to use out-of-the-box once the main Xamarin.Forms platform is initialized.
            // The Fingerprint plugin also does not require the SetCurrentActivityResolver on iOS.

            var dbFile = "settings_db.db3";
            // For iOS, store the database in the Library folder
            var folderPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "..", "Library");
            var dbFileCompletePath = System.IO.Path.Combine(folderPath, dbFile);

            LoadApplication(new App(dbFileCompletePath));

            return base.FinishedLaunching(app, options);
        }
    }
}