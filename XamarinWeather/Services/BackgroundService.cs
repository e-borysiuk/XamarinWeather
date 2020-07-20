using System;
using System.Threading;
using System.Threading.Tasks;
using Shiny;
using Shiny.Jobs;
using Shiny.Logging;
using Splat;
using Xamarin.Essentials;

namespace XamarinWeather.Services
{
    public class BackgroundService : IBackgroundService
    {
        private const string JobName = "BackgroundWeather";
        private readonly IJobManager _jobManager;
        public bool IsJobRunning { get; set; }

        public BackgroundService()
        {
            _jobManager = ShinyHost.Resolve<IJobManager>();
        }

        public async Task RunJob(int interval)
        {
            if (IsJobRunning)
                await StopJob();
            //Scheduled jobs can't be run in intervals lower than 15 minutes
            if (interval <= 15)
                interval = 15;
            var job = new JobInfo(typeof(WeatherJob), JobName)
            {
                PeriodicTime = TimeSpan.FromMinutes(interval),
                Repeat = true
            };
            await _jobManager.Schedule(job);
            IsJobRunning = true;
        }

        public async Task StopJob()
        {
            if (await _jobManager.GetJob(JobName) != null)
            {
                await _jobManager.Cancel(JobName);
                IsJobRunning = false;
            }
        }
    }

    public class WeatherJob : IJob
    {
        private readonly IWeatherService _weatherService;

        public WeatherJob()
        {
            _weatherService = Locator.Current.GetService<IWeatherService>();
        }

        public async Task<bool> Run(JobInfo jobInfo, CancellationToken cancelToken)
        {
            await _weatherService.RepeatLastQuery();
            return true;
        }
    }
}