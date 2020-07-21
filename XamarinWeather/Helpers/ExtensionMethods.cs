using System;
using System.Threading.Tasks;
using XamarinWeather.Models;

namespace XamarinWeather.Helpers
{
    public static class ExtensionMethods
    {
        public static double Round(this double d)
        {
            return Math.Round(d);
        }

        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }

        public static WeatherHistory ToHistory(this WeatherRoot weather)
        {
            return new WeatherHistory
            {
                Icon = $"_{weather?.Weather?[0]?.Icon}.png",
                LocationDetails = $"{weather.Name} {weather.DisplayDate}",
                ShortenedWeather = $"Temp: {weather.DisplayTemp}, {weather.DisplayDescription}"
            };
        }
    }
}