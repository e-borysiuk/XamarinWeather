using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinWeather.Models;
using XamarinWeather.Views;
using XamarinWeather.ViewModels;

namespace XamarinWeather.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class HistoryPage : ReactiveContentPage<HistoryViewModel>
    {
        public HistoryPage()
        {
            InitializeComponent();
        }
    }
}