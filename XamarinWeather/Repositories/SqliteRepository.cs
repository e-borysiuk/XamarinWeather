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

    public class SqliteRepository : IDataRepository, IDisposable
    {
        private static readonly Lazy<SQLiteAsyncConnection> LazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });
        private static SQLiteAsyncConnection Database => LazyInitializer.Value;
        private static bool _initialized;

        private readonly SourceCache<WeatherHistory, int> _internalSourceCache;
        public IObservableCache<WeatherHistory, int> History => _internalSourceCache;


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
            _internalSourceCache = new SourceCache<WeatherHistory, int>(o => o.Id);
        }

        public async Task<List<WeatherHistory>> GetItemsAsync()
        {
            var data = await Database.Table<WeatherHistory>().ToListAsync();
            if(_internalSourceCache.Count == 0)
            {
                _internalSourceCache.PopulateFrom(data.ToObservable());
            }
            return data;
        }

        public Task<WeatherHistory> GetItemAsync(int id)
        {
            return Database.Table<WeatherHistory>().Where(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(WeatherHistory item)
        {
            await Database.InsertAsync(item);
            string sql = @"select last_insert_rowid()";
            var insertedId = await Database.ExecuteScalarAsync<long>(sql);
            item.Id = (int)insertedId;
            _internalSourceCache.AddOrUpdate(item);
            return item.Id;
        }

        public Task<int> DeleteItemAsync(WeatherHistory item)
        {
            return Database.DeleteAsync(item);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _internalSourceCache?.Dispose();
            }
        }
    }
}