using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using XamarinWeather.Models;

namespace XamarinWeather.Services
{
    public interface IWeatherService
    {
        Task<WeatherRoot> GetWeather(double latitude, double longitude);
        Task<WeatherRoot> GetWeather(string city);
        Task RepeatLastQuery();
        Subject<WeatherRoot> NewWeatherUpdate { get; set; }
    }
}