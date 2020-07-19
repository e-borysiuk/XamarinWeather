using System;
using System.ComponentModel;
using System.Reactive.Disposables;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.ViewModels;

namespace XamarinWeather.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ReactiveTabbedPage<MainViewModel>
    {
        public MainPage(MainViewModel viewModel)
        {
            ViewModel = viewModel;

            this.WhenActivated(
                disposables =>
                {
                    this
                        .OneWayBind(this.ViewModel, 
                            x => x.HomeViewModel, 
                            x => x.HomePage.ViewModel)
                        .DisposeWith(disposables);
                    this
                        .OneWayBind(this.ViewModel, 
                            x => x.HistoryViewModel, 
                            x => x.HistoryPage.ViewModel)
                        .DisposeWith(disposables);
                });

            InitializeComponent();
        }
    }
}