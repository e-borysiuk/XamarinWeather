using System;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Splat;
using XamarinWeather.Models;
using static Newtonsoft.Json.JsonConvert;


namespace XamarinWeather.Services
{
    public class WeatherService : IWeatherService
    {
        const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=22b4d3a44013a5bc3ffacd3ee3a2048e";
        const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid=22b4d3a44013a5bc3ffacd3ee3a2048e";

        private string _lastSearchedCity;
        public Subject<WeatherRoot> NewWeatherUpdate { get; set; }

        public WeatherService()
        {
            NewWeatherUpdate = new Subject<WeatherRoot>();
        }

        public async Task<WeatherRoot> GetWeather(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCoordinatesUri, latitude, longitude);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var deserializedWeather = DeserializeObject<WeatherRoot>(json);
                _lastSearchedCity = deserializedWeather.Name;
                return deserializedWeather;
            }
        }

        public async Task<WeatherRoot> GetWeather(string city)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCityUri, city);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                var deserializedWeather = DeserializeObject<WeatherRoot>(json);
                _lastSearchedCity = deserializedWeather.Name;
                return deserializedWeather;
            }
        }

        public async Task RepeatLastQuery()
        {
            if (string.IsNullOrEmpty(_lastSearchedCity))
                return;
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCityUri, _lastSearchedCity);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return;

                var weather = DeserializeObject<WeatherRoot>(json);
                NewWeatherUpdate.OnNext(weather);
            }
        }
    }
}