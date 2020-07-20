using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.Helpers;
using XamarinWeather.Models;
using XamarinWeather.Services;

namespace XamarinWeather.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        WeatherService WeatherService { get; } = new WeatherService();
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        private readonly ObservableAsPropertyHelper<WeatherRoot> _output;
        private string _cityInput;
        private double _latitude;
        private double _longitude;

        public HomeViewModel()
        {
            Title = "Homepage";
            _output = ObservableAsPropertyHelper<WeatherRoot>.Default();

            GetWeather = ReactiveCommand.CreateFromTask<string, WeatherRoot>(
                city => {
                    if (string.IsNullOrEmpty(city))
                        return GetWeatherUsingGps();
                    return WeatherService.GetWeather(city);
                });

            _output = GetWeather.ToProperty(this, x => 
                x.Output, scheduler: RxApp.MainThreadScheduler);

            _isLoading =
                GetWeather
                    .IsExecuting
                    .ToProperty(this, x => x.IsLoading);

            //GetWeather.ThrownExceptions.Subscribe(exception => {
            //    this.Log().Warn("Error!", exception);
            //});
        }

        private async Task<WeatherRoot> GetWeatherUsingGps()
        {   
            var hasPermission = await PermissionHelpers.CheckPermissions();
            if (!hasPermission)
                return null;

            var position = await Geolocation.GetLastKnownLocationAsync();

            if (position == null)
            {
                // get full location if not cached.
                position = await Geolocation.GetLocationAsync(new GeolocationRequest
                {
                    DesiredAccuracy = GeolocationAccuracy.Medium,
                    Timeout = TimeSpan.FromSeconds(10)
                });
            }

            return await WeatherService.GetWeather(Latitude, Longitude);
        }

        public double Latitude
        {
            get => _latitude;
            set => this.RaiseAndSetIfChanged(ref _latitude, value);
        }
        public double Longitude
        {
            get => _longitude;
            set => this.RaiseAndSetIfChanged(ref _longitude, value);
        }

        public string CityInput
        {
            get => _cityInput;
            set => this.RaiseAndSetIfChanged(ref _cityInput, value);
        }

        public WeatherRoot Output => _output.Value;

        public bool IsLoading => _isLoading.Value;

        public ReactiveCommand<string, WeatherRoot> GetWeather { get; }
    }
}