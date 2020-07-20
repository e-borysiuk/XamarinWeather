using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
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
                    return WeatherService.GetWeather(city);
                return WeatherService.GetWeather(Latitude, Longitude);
            });

            _output = GetWeather.ToProperty(this, x => 
                x.Output, scheduler: RxApp.MainThreadScheduler);

            _isLoading =
                GetWeather
                    .IsExecuting
                    .ToProperty(this, x => x.IsLoading);

            GetWeather.ThrownExceptions.Subscribe(exception => {
                this.Log().Warn("Error!", exception);
            });
        }

        private void GetPosition()
        {

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