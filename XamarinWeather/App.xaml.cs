using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinWeather.Services;
using XamarinWeather.Views;

using Splat;
namespace XamarinWeather
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            var bootstrap = new AppBootstrapper();
            MainPage = new MainPage(bootstrap.CreateMainViewModel());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
