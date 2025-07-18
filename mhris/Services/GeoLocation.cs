using Plugin.Fingerprint.Abstractions;
using Plugin.Fingerprint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace mhris.Services
{
	public class GeoLocation
	{
		public async Task<Location> GetCurrentLocationAsync()
		{
			try
			{
				var location = await Geolocation.GetLastKnownLocationAsync();
				if (location == null)
				{
					location = await Geolocation.GetLocationAsync(new GeolocationRequest
					{
						DesiredAccuracy = GeolocationAccuracy.Medium,
						Timeout = TimeSpan.FromSeconds(30)
					});
				}
				return location;
			}
			catch
			{
				// Handle exceptions
				return null;
			}
		}

		public bool IsLocationMatch(Location currentLocation, double targetLatitude, double targetLongitude, double threshold = 0.01)
		{
			if (currentLocation == null)
				return false;

			var distance = Location.CalculateDistance(currentLocation, new Location(targetLatitude, targetLongitude), DistanceUnits.Kilometers);
			return distance <= threshold;
		}

		public async Task<bool> AuthenticateWithFingerprintAsync()
		{
			var result = await CrossFingerprint.Current.AuthenticateAsync(new AuthenticationRequestConfiguration("Authentication", "Prove you have fingers!"));
			return result.Authenticated;
		}

		public async Task<bool> AuthenticateUserAsync(double targetLatitude, double targetLongitude)
		{
			var currentLocation = await GetCurrentLocationAsync();
			if (IsLocationMatch(currentLocation, targetLatitude, targetLongitude))
			{
				return await AuthenticateWithFingerprintAsync();
			}
			return false;
		}

		private async void OnAuthenticateButtonClicked(object sender, EventArgs e)
		{
			double targetLatitude = 37.7749; // Example latitude
			double targetLongitude = -122.4194; // Example longitude

			bool isAuthenticated = await AuthenticateUserAsync(targetLatitude, targetLongitude);
			if (isAuthenticated)
			{
				// Proceed with the next steps after successful authentication
			}
			else
			{
				// Handle authentication failure
			}
		}
	}
}
