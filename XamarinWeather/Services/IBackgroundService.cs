using System.Threading.Tasks;

namespace XamarinWeather.Services
{
    public interface IBackgroundService
    {
        Task RunJob(int interval);
        Task StopJob();
        bool IsJobRunning { get; }
    }
}