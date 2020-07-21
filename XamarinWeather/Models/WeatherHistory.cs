using System;
using SQLite;
using Xamarin.Forms;

namespace XamarinWeather.Models
{
    public class WeatherHistory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string ShortenedWeather { get; set; }

        public string LocationDetails { get; set; }

        public string Icon { get; set; }
    }
}