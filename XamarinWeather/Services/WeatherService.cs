using System.Net.Http;
using System.Threading.Tasks;
using XamarinWeather.Models;
using static Newtonsoft.Json.JsonConvert;


namespace XamarinWeather.Services
{
    public class WeatherService
    {
        const string WeatherCoordinatesUri = "http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&units=metric&appid=22b4d3a44013a5bc3ffacd3ee3a2048e";
        const string WeatherCityUri = "http://api.openweathermap.org/data/2.5/weather?q={0}&units=metric&appid=22b4d3a44013a5bc3ffacd3ee3a2048e";

        public async Task<WeatherRoot> GetWeather(double latitude, double longitude)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(WeatherCoordinatesUri, latitude, longitude);
                var json = await client.GetStringAsync(url);

                if (string.IsNullOrWhiteSpace(json))
                    return null;

                return DeserializeObject<WeatherRoot>(json);
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

                return DeserializeObject<WeatherRoot>(json);
            }

        }
    }
}