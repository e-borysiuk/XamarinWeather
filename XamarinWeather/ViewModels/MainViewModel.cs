using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;

namespace XamarinWeather.ViewModels
{
    public class MainViewModel : ReactiveObject, IScreen
    {
        public HistoryViewModel HistoryViewModel => new HistoryViewModel();

        public HomeViewModel HomeViewModel => new HomeViewModel();

        public MainViewModel()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
        }

        public RoutingState Router { get; }
    }
}