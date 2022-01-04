using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RentReady.Common.Options;
using RentReady.DAL;

[assembly: FunctionsStartup(typeof(RentReady.Functions.Startup))]

namespace RentReady.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddDataAccessLayer();

            builder.Services.AddOptions<Dataverse>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("values:" + nameof(Dataverse)).Bind(settings); });
           
        }
    }
}