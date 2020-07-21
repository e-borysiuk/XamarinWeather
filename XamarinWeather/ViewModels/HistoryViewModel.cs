using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.PLinq;
using ReactiveUI;
using Splat;
using XamarinWeather.Models;
using XamarinWeather.Repositories;

namespace XamarinWeather.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        private readonly IDataRepository _dataRepository;
        private readonly ObservableAsPropertyHelper<bool> _isRefreshing;
        private readonly ReadOnlyObservableCollection<HistoryCellViewModel> _history;

        public HistoryViewModel()
        {
            Title = "History";
            _dataRepository = Locator.Current.GetService<IDataRepository>();

            LoadHistory = ReactiveCommand.CreateFromTask<Unit, Unit>(async _ => {
                await _dataRepository.GetItemsAsync();
                return Unit.Default;
            });

            _dataRepository
                .History
                .Connect()
                .SubscribeOn(TaskPoolScheduler)
                .ObserveOn(TaskPoolScheduler)
                .Transform(movie => new HistoryCellViewModel(movie))
                .DisposeMany()
                .ObserveOn(MainThreadScheduler)
                .Bind(out _history)
                .Subscribe();

            LoadHistory.Subscribe();
            LoadHistory.Execute();

            _isRefreshing =
                LoadHistory
                    .IsExecuting
                    .ToProperty(this, x => x.IsRefreshing, true);
        }
        public ReadOnlyObservableCollection<HistoryCellViewModel> Movies => _history;

        public ReactiveCommand<Unit, Unit> LoadHistory
        {
            get;
        }

        public bool IsRefreshing => _isRefreshing.Value;
    }
}