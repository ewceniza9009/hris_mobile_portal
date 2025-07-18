using mhris.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace mhris
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void CmdLogin_Clicked(object sender, EventArgs e)
        {
            LoginActivityIndicator.IsRunning = true;
            LoginActivityIndicator.IsVisible = true;

            var result = await AppRequest.IsLoginValid(LoginCode.Text);
            Global.LoginCode = LoginCode.Text;

            if (result.Item1)
            {
                if (!File.Exists(App.FilePath))
                {
                    SettingRepos.CreateLocalSettingDB(LoginCode.Text);
                }
                else 
                {
                    SettingRepos.UpdateLocalSettingDB(LoginCode.Text);
                }

                //await Application.Current.MainPage
                //   .DisplayAlert("Login", "Succesfully Logged-In.", "OK");

                Device.BeginInvokeOnMainThread(async () => 
                {
                    await Navigation.PushAsync(new Views.Payroll());
                    //.ContinueWith(x => {
                    //    LoginMessage.Text = "";

                    //    LoginActivityIndicator.IsRunning = false;
                    //    LoginActivityIndicator.IsVisible = false;

                    //    var completed = x.IsCompleted;
                    //});

                    LoginMessage.Text = "";

                    LoginActivityIndicator.IsRunning = false;
                    LoginActivityIndicator.IsVisible = false;
                });
            }
            else
            {
                LoginMessage.Text = "Invalid login, Please try again.";

                if (result.Item2 != "Success") 
                {
                    LoginMessage.Text = $"{result.Item2}";
                }

                LoginActivityIndicator.IsRunning = false;
                LoginActivityIndicator.IsVisible = false;
            }
        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            LoginCode.IsEnabled = true;

            if (File.Exists(App.FilePath))
            {
                var setting = SettingRepos.GetSettings();

                if (!string.IsNullOrEmpty((setting?.LoginCode ?? string.Empty)))
                {
                    LoginCode.IsEnabled = false;

                    LoginActivityIndicator.IsRunning = true;
                    LoginActivityIndicator.IsVisible = true;
                }
                else 
                {
                    LoginCode.Text = string.Empty;
                    return;
                }

                var result = await AppRequest.IsLoginValid(setting?.LoginCode ?? "000000");

                Global.LoginCode = setting.LoginCode ?? "000000";

                if (result.Item1)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushAsync(new Views.Payroll());

                        LoginMessage.Text = "";

                        LoginActivityIndicator.IsRunning = false;
                        LoginActivityIndicator.IsVisible = false;
                    });
                }
                else 
                {
                    if (result.Item2 != "Success")
                    {
                        LoginMessage.Text = $"{result.Item2}";
                    }
                }
            }
        }
    }
}
