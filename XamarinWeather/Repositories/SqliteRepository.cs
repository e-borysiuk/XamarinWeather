using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using DynamicData;
using SQLite;
using XamarinWeather.Helpers;
using XamarinWeather.Models;

namespace XamarinWeather.Repositories
{

    public class SqliteRepository : IDataRepository
    {
        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        private static SQLiteAsyncConnection Database => LazyInitializer.Value;
        private static bool _initialized;


        private async Task InitializeAsync()
        {
            if (!_initialized)
            {
                if (Database.TableMappings.All(m => m.MappedType.Name != typeof(WeatherHistory).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(WeatherHistory)).ConfigureAwait(false);
                    _initialized = true;
                }
            }
        }

        public SqliteRepository()
        {
            InitializeAsync().SafeFireAndForget(false);
            NewEntryUpdate = new Subject<int>();
        }

        public Subject<int> NewEntryUpdate { get; set; }

        public Task<List<WeatherHistory>> GetItemsAsync()
        {
            return Database.Table<WeatherHistory>().ToListAsync();
        }

        public Task<WeatherHistory> GetItemAsync(int id)
        {
            return Database.Table<WeatherHistory>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(WeatherHistory item)
        {
            NewEntryUpdate.OnNext(1);
            if (item.Id != 0)
            {
                return Database.UpdateAsync(item);
            }
            return Database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(WeatherHistory item)
        {
            return Database.DeleteAsync(item);
        }
    }
}