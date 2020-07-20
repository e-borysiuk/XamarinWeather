using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinWeather.Helpers
{
    public static class PermissionHelpers
    {
        public static async Task<bool> CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            var request = false;
            if (status == PermissionStatus.Denied)
            {
                if (Device.RuntimePlatform == Device.iOS)
                {

                    var title = "Location Permission";
                    var question =
                        "To get your current city the location permission is required. Please go into Settings and turn on Location for the app.";
                    var positive = "Settings";
                    var negative = "Maybe Later";
                    var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                    if (task == null)
                        return false;

                    var result = await task;
                    if (result)
                    {
                        AppInfo.ShowSettingsUI();
                    }

                    return false;
                }

                request = true;
            }

            if (!request && status == PermissionStatus.Granted) return true;
            {
                var newStatus = await Permissions.RequestAsync<Permissions.LocationAlways>();
                if (newStatus == PermissionStatus.Granted) return true;
                var title = "Location Permission";
                var question = "To get your current city the location permission is required.";
                var positive = "Settings";
                var negative = "Maybe Later";
                var task = Application.Current?.MainPage?.DisplayAlert(title, question, positive, negative);
                if (task == null)
                    return false;

                var result = await task;
                if (result)
                {
                    AppInfo.ShowSettingsUI();
                }

                return false;
            }
        }
    }
}