using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData.Binding;
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
        private readonly IWeatherService _weatherService;
        private readonly IBackgroundService _backgroundService;
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        //private readonly ObservableAsPropertyHelper<WeatherRoot> _output;
        private WeatherRoot _output;
        private string _cityInput;
        private string _interval;

        public HomeViewModel()
        {
            Interval = "15";
            Title = "Homepage";
            _weatherService = Locator.Current.GetService<IWeatherService>();
            _backgroundService = Locator.Current.GetService<IBackgroundService>();

            GetWeather = ReactiveCommand.CreateFromTask<string, WeatherRoot>(
                city => {
                    if (string.IsNullOrEmpty(city))
                        return GetWeatherUsingGps();
                    return _weatherService.GetWeather(city);
                });

            GetWeather.Subscribe(data => { Output = data; });

            _weatherService.NewWeatherUpdate.Subscribe(data => { Output = data; });

            _isLoading = GetWeather
                    .IsExecuting
                    .ToProperty(this, x => x.IsLoading);

            this.WhenValueChanged(x => x.Interval)
                .Skip(1)
                .Throttle(TimeSpan.FromSeconds(2))
                .Where(x => !string.IsNullOrEmpty(x))
                .Subscribe(interval =>
                {
                    _backgroundService.StopJob();
                    _backgroundService.RunJob(int.Parse(interval));
                });

            GetWeather.IsExecuting
                .Skip(1) // IsExecuting has an initial value of false.  We can skip that first value
                .Where(isExecuting => !isExecuting) // filter until the executing state becomes false
                .Subscribe(_ => {
                    if(!_backgroundService.IsJobRunning)
                        _backgroundService.RunJob(int.Parse(string.IsNullOrEmpty(Interval) ? "0" : Interval));
                });
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

            return await _weatherService.GetWeather(position.Latitude, position.Longitude);
        }

        public string CityInput
        {
            get => _cityInput;
            set => this.RaiseAndSetIfChanged(ref _cityInput, value);
        }
        public string Interval
        {
            get => _interval;
            set => this.RaiseAndSetIfChanged(ref _interval, value);
        }
        public WeatherRoot Output
        {
            get => _output;
            set => this.RaiseAndSetIfChanged(ref _output, value);
        }
        public bool IsLoading => _isLoading.Value;
        public ReactiveCommand<string, WeatherRoot> GetWeather { get; }
    }
}