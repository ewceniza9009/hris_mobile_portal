using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using Android.Window;

namespace mhris.Droid
{
    [Activity(Label = "mhris", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

			// Initialize Current Activity Plugin
			CrossCurrentActivity.Current.Init(this, savedInstanceState);

			// Set the Current Activity Resolver for Fingerprint Plugin
			Plugin.Fingerprint.CrossFingerprint.SetCurrentActivityResolver(() => CrossCurrentActivity.Current.Activity);

			var dbFile = "settings_db.db3";
            var folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var dbFileCompletePath = System.IO.Path.Combine(folderPath, dbFile);

            LoadApplication(new App(dbFileCompletePath));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

		public override void OnBackPressed()
		{
			if (CrossCurrentActivity.Current.Activity != null && !CrossCurrentActivity.Current.Activity.IsFinishing)
			{
				CrossCurrentActivity.Current.Activity.Finish();
			}
			else
			{
				base.OnBackPressed();
			}
		}

		//public override void OnBackPressed()
		//{
		//    base.OnBackPressed();
		//    return;
		//}
	}
}