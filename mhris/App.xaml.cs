using Syncfusion.Licensing;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace mhris
{
    public partial class App : Application
    {
        public static string FilePath;
        public App(string filePath)
        {
            InitializeComponent();

            SyncfusionLicenseProvider.RegisterLicense("MzcxM0AzMjMwMkUzNDJFMzBpYnpCR0U4NjhVTjR2QWFIRkZHa2VHOGI3N1JRYlFKQ3dYbk5iTE9JTmdFPQ==");

            FilePath = filePath;

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
