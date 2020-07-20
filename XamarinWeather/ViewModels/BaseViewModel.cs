using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reactive.Concurrency;
using System.Runtime.CompilerServices;
using ReactiveUI;
using Xamarin.Forms;

using XamarinWeather.Models;
using XamarinWeather.Services;

namespace XamarinWeather.ViewModels
{
    public class BaseViewModel : ReactiveObject
    {
        public IDataStore<Item> DataStore;

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => this.RaiseAndSetIfChanged(ref isBusy, value);
        }

        private string title;
        public string Title
        {
            get => title;
            set => this.RaiseAndSetIfChanged(ref title, value);
        }

        public BaseViewModel(IScheduler mainThreadScheduler = null, IScheduler taskPoolScheduler = null)
        {
            DataStore = DependencyService.Get<IDataStore<Item>>(); 
            MainThreadScheduler = mainThreadScheduler ?? RxApp.MainThreadScheduler;
            TaskPoolScheduler = taskPoolScheduler ?? RxApp.TaskpoolScheduler;
        }

        protected IScheduler MainThreadScheduler { get; }

        protected IScheduler TaskPoolScheduler { get; }
    }
}
