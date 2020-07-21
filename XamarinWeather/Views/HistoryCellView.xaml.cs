using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.ViewModels;

namespace XamarinWeather.Views
{
    [DesignTimeVisible(false)]
    public partial class HistoryCellView : ReactiveViewCell<HistoryCellViewModel>
    {
        private readonly CompositeDisposable _subscriptionDisposables = new CompositeDisposable();

        public HistoryCellView()
        {
            InitializeComponent();


            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel,
                        x => x.ShortenedWeather,
                        x => x.LbTitle.Text, x => x)
                    .DisposeWith(_subscriptionDisposables);
                this.OneWayBind(ViewModel, 
                    x => x.LocationDetails, 
                    x => x.LbDescription.Text)
                    .DisposeWith(_subscriptionDisposables);
                //this.OneWayBind(ViewModel, 
                //    x => x.Icon, 
                //    x => x.ImIcon.Source, x => x)
                //    .DisposeWith(_subscriptionDisposables);
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _subscriptionDisposables.Clear();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            ImIcon.Source = null;

            if (!(BindingContext is HistoryCellViewModel item))
            {
                return;
            }

            ImIcon.Source = item.Icon;
        }
    }
}