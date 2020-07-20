namespace XamarinWeather.Droid
{
    using System;
    using Shiny;
    using Android.App;
    using Android.Runtime;


    namespace YourNamespace
    {
        [Application]
        public class MainApplication : ShinyAndroidApplication<Startup>
        {
            public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
            {
            }

            public override void OnCreate()
            {
                base.OnCreate();

                Shiny.AndroidShinyHost.Init(
                    this,
                    new Startup()
                );
            }
        }
    }
}