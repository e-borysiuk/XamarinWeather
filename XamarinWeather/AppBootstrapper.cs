﻿using ReactiveUI;
using Splat;
using XamarinWeather.ViewModels;
using XamarinWeather.Views;

namespace XamarinWeather
{
    public class AppBootstrapper
    {
        public AppBootstrapper()
        {
            RegisterViews();
            RegisterViewModels();
        }

        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomeViewModel>));
            Locator.CurrentMutable.Register(() => new HistoryPage(), typeof(IViewFor<HistoryViewModel>));
        }

        private void RegisterViewModels()
        {
            Locator.CurrentMutable.Register(() => new HomeViewModel(), typeof(IRoutableViewModel), typeof(HomeViewModel).FullName);
            Locator.CurrentMutable.Register(() => new HistoryViewModel(), typeof(IRoutableViewModel), typeof(HistoryViewModel).FullName);
        }

        public MainViewModel CreateMainViewModel()
        {
            var viewModel = new MainViewModel();

            return viewModel;
        }
    }
}