using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using ReactiveUI;
using Splat;
using Xamarin.Forms;

using XamarinWeather.Models;
using XamarinWeather.Repositories;
using XamarinWeather.Services;
using XamarinWeather.Views;

namespace XamarinWeather.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IDataRepository _dataRepository;
        private readonly ObservableAsPropertyHelper<bool> _isRefreshing;
        private IEnumerable<HistoryCellViewModel> _history;

        public HistoryViewModel()
        {
            Title = "History";
            _dataRepository = Locator.Current.GetService<IDataRepository>();

            LoadHistory = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ => {
                    var data = await _dataRepository.GetItemsAsync();
                    History = new List<HistoryCellViewModel>(data.Select(x => new HistoryCellViewModel(x)));
                    return Unit.Default;
                }, outputScheduler: RxApp.TaskpoolScheduler);

            LoadHistory.Subscribe();

            _dataRepository.NewEntryUpdate.ObserveOn(RxApp.MainThreadScheduler).Subscribe(_ =>
            {
                LoadHistory.Execute();
            });

            _isRefreshing =
                LoadHistory
                    .IsExecuting
                    .ToProperty(this, x => x.IsRefreshing, true);
        }

        public IEnumerable<HistoryCellViewModel> History
        {
            get => _history;
            set => this.RaiseAndSetIfChanged(ref _history, value);
        }

        public ReactiveCommand<Unit, Unit> LoadHistory
        {
            get;
        }

        public bool IsRefreshing => _isRefreshing.Value;
    }
}