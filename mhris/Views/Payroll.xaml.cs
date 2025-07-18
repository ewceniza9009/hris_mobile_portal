using mhris.Models;
using mhris.Services;
using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;

namespace mhris.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Payroll : ContentPage, INotifyPropertyChanged
    {
		public new event PropertyChangedEventHandler PropertyChanged;

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);

            return true;
        }

		//private const double targetLatitude = 10.3715267; // Example latitude
		//private const double targetLongitude = 123.9344896; // Example longitude
		//private const double updateDistanceThreshold = 0.002; // 10 meters
		//private const int locationUpdateInterval = 5000; // 10 seconds
		//private bool isLocationUpdating;

		private const double updateDistanceThreshold = 0.002; // 2 meters
		private const int locationUpdateInterval = 5000; // 10 seconds
		private bool isLocationUpdating;

		public double targetLatitude 
		{
			get => _targetLatitude;
			set 
			{
				_targetLatitude = value;
				OnPropertyChanged(nameof(targetLatitude));
			} 
		}
		private double _targetLatitude = 10.3715267;

		public double targetLongitude
		{
			get => _targetLongitude;
			set
			{
				_targetLongitude = value;
				OnPropertyChanged(nameof(targetLongitude));
			}
		}
		private double _targetLongitude = 123.9344896;

		public List<PayrollRecord> PayrollNumbers
        {
            get
            {
                return _PayrollNumbers;
            }
            set
            {
                _PayrollNumbers = value;
                OnPropertyChanged(nameof(PayrollNumbers));
            }
        }
        private List<PayrollRecord> _PayrollNumbers;

        public PayrollRecord SelectedPayrollNumber
        {
            get
            {
                return _SelectedPayrollNumber;
            }
            set
            {
                _SelectedPayrollNumber = value;
                OnPropertyChanged(nameof(SelectedPayrollNumber));
            }
        }
        private PayrollRecord _SelectedPayrollNumber;

        public PaySlipRecord PaySlip 
        {
            get 
            {
                return _PaySlip;
            } 
            set 
            {
                _PaySlip = value;
                OnPropertyChanged(nameof(PaySlip));
            }
        }
        private PaySlipRecord _PaySlip;

        public List<DTRRecord> DTRNumbers
        {
            get
            {
                return _DTRNumbers;
            }
            set
            {
                _DTRNumbers = value;
                OnPropertyChanged(nameof(DTRNumbers));
            }
        }
        private List<DTRRecord> _DTRNumbers;

        public  DTRRecord SelectedDTR
        {
            get
            {
                return _SelectedDTR;
            }
            set
            {
                _SelectedDTR = value;
                OnPropertyChanged(nameof(SelectedDTR));
            }
        }
        private DTRRecord _SelectedDTR;

        public List<DTRSlipRecord> DTRSlip
        {
            get
            {
                return _DTRSlip;
            }
            set
            {
                _DTRSlip = value;
                OnPropertyChanged(nameof(DTRSlip));
            }
        }
        private List<DTRSlipRecord> _DTRSlip;

        void FitOtherDeductionBreakdown() 
        {
            OtherDeductionBreakdown.WidthRequest = StackOtherDeductionBreakdown.Width;
        }

        public Payroll()
        {
            InitializeComponent();

            BindingContext = this;

			txtTargetLatitude.Text = targetLatitude.ToString();
			txtTargetLongitude.Text = targetLongitude.ToString();

			LoadMap();
			//StartLocationUpdates();
		}

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            var stringCode = Global.LoginCode;

            PayrollNumbers = AppRequest.GetPayrollNumbers(stringCode);
            var dtrSettings= AppRequest.GetDTRs(stringCode);
            DTRNumbers = dtrSettings.Value;

            SelectedPayrollNumber = PayrollNumbers.FirstOrDefault();
            var defaultPayrollId = PayrollNumbers?.FirstOrDefault()?.Id ?? 0;

            SelectedDTR = DTRNumbers.FirstOrDefault();
            var defaultDTRId = DTRNumbers?.FirstOrDefault()?.Id ?? 0;

            var list = await AppRequest.GetPayslipByPayrollIdAndEmployeeCode(defaultPayrollId, stringCode);
            var listDTR = await AppRequest.GetDTRSlipByDTRIdAndEmployeeCode(defaultDTRId, stringCode);

            PaySlip = list.FirstOrDefault();
            DTRSlip = listDTR;

            FitOtherDeductionBreakdown();
        }       

        private async void PayrollId_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var stringCode = Global.LoginCode;
            var payrollId = SelectedPayrollNumber.Id;

            var list = await AppRequest.GetPayslipByPayrollIdAndEmployeeCode(payrollId, stringCode);
            PaySlip = list.FirstOrDefault();

            FitOtherDeductionBreakdown();

            OnPropertyChanged(nameof(PaySlip));
        }

        private async void DTRId_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            var stringCode = Global.LoginCode;
            var dtrId = SelectedDTR.Id;

            var listDTR = await AppRequest.GetDTRSlipByDTRIdAndEmployeeCode(dtrId, stringCode);
            DTRSlip = listDTR;

            OnPropertyChanged(nameof(DTRSlip));
        }

        private async void SfButton_Clicked(object sender, EventArgs e)
        {
            SettingRepos.UpdateLocalSettingDB(string.Empty);

            await Navigation.PopAsync();
        }

		private async void StartTrack_Clicked(object sender, EventArgs e)
		{
			if (!(chkIn?.IsChecked ?? false) && !(chkBreakOut?.IsChecked ?? false) && !(chkBreakIn?.IsChecked ?? false) && !(chkOut?.IsChecked ?? false))
			{
				await DisplayAlert("Log Type", "Please select a log type.", "OK");
			}
			else
			{
				await SetTargetLocation();
				StartLocationUpdates();
				mhrisTab.SelectedIndex = 1;
			}
		}

		private void StopBiometric_Clicked(object sender, EventArgs e)
		{
			StopLocationUpdates();
		}

		private void chkIn_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
		{
            chkBreakOut.StateChanged -= chkBreakOut_StateChanged;
            chkBreakOut.IsChecked = false;
			chkBreakOut.StateChanged += chkBreakOut_StateChanged;
			chkBreakIn.StateChanged -= chkBreakIn_StateChanged;
			chkBreakIn.IsChecked = false;
			chkBreakIn.StateChanged += chkBreakIn_StateChanged;
            chkOut.StateChanged -= chkOut_StateChanged;
			chkOut.IsChecked = false;
			chkOut.StateChanged += chkOut_StateChanged;
		}

		private void chkBreakOut_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
		{
			chkIn.StateChanged -= chkIn_StateChanged;
			chkIn.IsChecked = false;
			chkIn.StateChanged += chkIn_StateChanged;
			chkBreakIn.StateChanged -= chkBreakIn_StateChanged;
			chkBreakIn.IsChecked = false;
			chkBreakIn.StateChanged += chkBreakIn_StateChanged;
			chkOut.StateChanged -= chkOut_StateChanged;
			chkOut.IsChecked = false;
			chkOut.StateChanged += chkOut_StateChanged;
		}

		private void chkBreakIn_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
		{
			chkIn.StateChanged -= chkIn_StateChanged;
			chkIn.IsChecked = false;
			chkIn.StateChanged += chkIn_StateChanged;
			chkBreakOut.StateChanged -= chkBreakOut_StateChanged;
			chkBreakOut.IsChecked = false;
			chkBreakOut.StateChanged += chkBreakOut_StateChanged;
			chkOut.StateChanged -= chkOut_StateChanged;
			chkOut.IsChecked = false;
			chkOut.StateChanged += chkOut_StateChanged;
		}

		private void chkOut_StateChanged(object sender, Syncfusion.XForms.Buttons.StateChangedEventArgs e)
		{
			chkIn.StateChanged -= chkIn_StateChanged;
			chkIn.IsChecked = false;
			chkIn.StateChanged += chkIn_StateChanged;
			chkBreakOut.StateChanged -= chkBreakOut_StateChanged;
			chkBreakOut.IsChecked = false;
			chkBreakOut.StateChanged += chkBreakOut_StateChanged;
			chkBreakIn.StateChanged -= chkBreakIn_StateChanged;
			chkBreakIn.IsChecked = false;
			chkBreakIn.StateChanged += chkBreakIn_StateChanged;
		}

		private async void ClearBiometric_Clicked(object sender, EventArgs e)
		{
			var result = await DisplayAlert("Biometrid", "Do you want to clear logs?", "Yes", "No");

			if (result) 
			{
				ClearLogCheckBoxes();
				SettingRepos.UpdateLocalSettingDBLog("CR");
			}
		}

		private void LoadMap()
		{
			var htmlSource = new HtmlWebViewSource();

			var assembly = typeof(MainPage).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("mhris.Map.html");
			using (var reader = new StreamReader(stream))
			{
				htmlSource.Html = reader.ReadToEnd();
			}

			mapWebView.Source = htmlSource;

			mapWebView.Navigated += async (s, e) =>
			{
				System.Diagnostics.Debug.WriteLine("WebView navigated to map.html");

				await Task.Delay(500);
				await SetTargetLocation();
			};
		}

		private async Task SetTargetLocation()
		{
			var setTargetScript = $"setTargetLocation({targetLatitude}, {targetLongitude});";
			System.Diagnostics.Debug.WriteLine("Setting target location via JavaScript");
			Error1.Text = "Setting target location via JavaScript";
			await mapWebView.EvaluateJavaScriptAsync(setTargetScript);
		}

		private async Task<Location> GetUserLocationAsync()
		{
			try
			{
				var location = await Geolocation.GetLocationAsync(new GeolocationRequest
				{
					DesiredAccuracy = GeolocationAccuracy.Best,
					Timeout = TimeSpan.FromSeconds(10) // Adjust timeout as needed
				});

				return location;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error getting user location: {ex.Message}");
				Error1.Text = $"Error getting user location: {ex.Message}";
				return null;
			}
		}

		private async void StartLocationUpdates()
		{
			isLocationUpdating = true;

			while (isLocationUpdating)
			{
				var location = await GetUserLocationAsync();
				if (location != null)
				{
					var script = $"updateUserLocation({location.Latitude}, {location.Longitude});";
					System.Diagnostics.Debug.WriteLine("Updating user location via JavaScript");
					await mapWebView.EvaluateJavaScriptAsync(script);

					var distance = Location.CalculateDistance(location, new Location(targetLatitude, targetLongitude), DistanceUnits.Kilometers);

					txtLatitude.Text = Math.Round(location.Latitude, 10).ToString();
					txtLongitude.Text = Math.Round(location.Longitude, 10).ToString();
					txtDistance.Text = Math.Round(distance, 10).ToString();

					var settings = SettingRepos.GetSettings();

					if (settings.IsSwipedIn == true && (chkIn?.IsChecked ?? false))
					{
						await DisplayAlert("Biometric", "Already swiped in", "OK");
						break;
					}

					if (settings.IsSwipedBreakStart == true && (chkBreakIn?.IsChecked ?? false))
					{
						await DisplayAlert("Biometric", "Already swiped break start", "OK");
						break;
					}

					if (settings.IsSwipedBreakEnd == true && (chkBreakOut?.IsChecked ?? false))
					{
						await DisplayAlert("Biometric", "Already swiped break end", "OK");
						break;
					}

					if (settings.IsSwipedOut == true && (chkOut?.IsChecked ?? false))
					{
						await DisplayAlert("Biometric", "Already swiped out", "OK");
						break;
					}

					if (distance <= updateDistanceThreshold)
					{
						await RequireFingerprintAuthentication();
						break;
					}
				}

				await Task.Delay(locationUpdateInterval); // Adjust this interval as needed
			}
		}

		private void StopLocationUpdates()
		{
			isLocationUpdating = false;
		}

		private async Task RequireFingerprintAuthentication()
		{
			var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Please input your finger", "This is a requirement for log checking."));
			if (result.Authenticated)
			{
				if (chkIn?.IsChecked ?? false) SettingRepos.UpdateLocalSettingDBLog("IN");
				if (chkBreakIn?.IsChecked ?? false) SettingRepos.UpdateLocalSettingDBLog("BS");
				if (chkBreakOut?.IsChecked ?? false) SettingRepos.UpdateLocalSettingDBLog("BE");
				if (chkOut?.IsChecked ?? false) SettingRepos.UpdateLocalSettingDBLog("OT");

				ClearLogCheckBoxes();

				StopLocationUpdates();

				await DisplayAlert("Success", "Fingerprint authentication succeeded", "OK");
			}
			else
			{
				await DisplayAlert("Failure", "Fingerprint authentication failed", "OK");
			}
		}

		private void ClearLogCheckBoxes()
		{
			chkIn.StateChanged -= chkIn_StateChanged;
			chkIn.IsChecked = false;
			chkIn.StateChanged += chkIn_StateChanged;
			chkBreakOut.StateChanged -= chkBreakOut_StateChanged;
			chkBreakOut.IsChecked = false;
			chkBreakOut.StateChanged += chkBreakOut_StateChanged;
			chkBreakIn.StateChanged -= chkBreakIn_StateChanged;
			chkBreakIn.IsChecked = false;
			chkBreakIn.StateChanged += chkBreakIn_StateChanged;
			chkOut.StateChanged -= chkOut_StateChanged;
			chkOut.IsChecked = false;
			chkOut.StateChanged += chkOut_StateChanged;
		}
	}
}