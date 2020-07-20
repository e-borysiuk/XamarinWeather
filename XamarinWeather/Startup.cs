using Microsoft.Extensions.DependencyInjection;
using Shiny;
using Shiny.Logging;
using XamarinWeather.Services;

namespace XamarinWeather
{
    public class Startup : ShinyStartup
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            Log.UseConsole();
            Log.UseDebug();
        }
    }
}