using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public BaseViewModel()
        {
            DataStore = DependencyService.Get<IDataStore<Item>>();
        }
    }
}
