using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.Helpers;
using XamarinWeather.ViewModels;

namespace XamarinWeather.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class HomePage : ReactiveContentPage<HomeViewModel>
    {
        public HomePage()
        {
            InitializeComponent();
            this.WhenActivated(disposable =>
            {
                this.BindCommand(
                    ViewModel,
                    x => x.GetWeather,
                    x => x.BtGetWeather,
                    x => x.CityInput).DisposeWith(disposable);
                
                this.Bind(ViewModel,
                    x => x.Interval,
                    x => x.EtInterval.Text).DisposeWith(disposable);

                this.Bind(ViewModel,
                    x => x.CityInput,
                    x => x.EtLocation.Text).DisposeWith(disposable);

                this.Bind(ViewModel,
                    x => x.Output.Name,
                    x => x.LbCityName.Text).DisposeWith(disposable);

                this.Bind(ViewModel,
                    x => x.Output.DisplayTemp,
                    x => x.LbTemp.Text).DisposeWith(disposable);
                
                this.OneWayBind(ViewModel, 
                    x => x.Output.DisplayHumidity,
                    x => x.LbHumidity.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel,
                    x => x.Output.DisplayPressure,
                    x => x.LbPressure.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel,
                    x => x.Output.DisplayDescription,
                    x => x.LbClouds.Text).DisposeWith(disposable);

                this.OneWayBind(ViewModel,
                    x => x.Output.DisplayIcon,
                    x => x.ImIcon.Source).DisposeWith(disposable);

                this.OneWayBind(ViewModel,
                    x => x.IsLoading,
                    x => x.AiLoading.IsRunning).DisposeWith(disposable);

                ViewModel.GetWeather.ThrownExceptions
                    .Subscribe(async exception =>
                    {
                        if (exception.Message.Contains("404"))
                            await DisplayAlert("Alert", "Couldn't find specified city.", "OK");
                        else
                            await DisplayAlert("Alert", "Something went wrong.", "OK");
                    })
                    .DisposeWith(disposable);
            });

            this.WhenAnyValue(x => x.ViewModel.CityInput)
                .Subscribe(x => {
                    this.BtGetWeather.Text = string.IsNullOrEmpty(x) ? "Get weather using GPS" : "Get weather for provided city";
                });

            this.WhenValueChanged(x => x.ViewModel.Output)
                .Where(x => x != null)
                .Subscribe(x => { this.GrWeather.IsVisible = true; });
        }
    }
}