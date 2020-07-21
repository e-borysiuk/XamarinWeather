using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using Xamarin.Essentials;
using XamarinWeather.Models;

namespace XamarinWeather.Repositories
{
    public interface IDataRepository
    {
        Task<List<WeatherHistory>> GetItemsAsync();
        Task<WeatherHistory> GetItemAsync(int id);
        Task<int> SaveItemAsync(WeatherHistory item);
        Task<int> DeleteItemAsync(WeatherHistory item);
        IObservableCache<WeatherHistory, int> History { get; }
    }
}