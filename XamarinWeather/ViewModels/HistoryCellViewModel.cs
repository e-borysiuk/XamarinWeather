using System;
using System.Globalization;
using ReactiveUI;
using Xamarin.Forms;
using XamarinWeather.Models;

namespace XamarinWeather.ViewModels
{
    public class HistoryCellViewModel : BaseViewModel
    {
        public HistoryCellViewModel(WeatherHistory weather)
        {
            Weather = weather;
        }

        public WeatherHistory Weather { get; }

        public string ShortenedWeather => Weather.ShortenedWeather;

        public string LocationDetails => Weather.LocationDetails;

        public ImageSource Icon => ImageSource.FromFile(Weather?.Icon);
    }
}