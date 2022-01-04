using Microsoft.Extensions.DependencyInjection;
using RentReady.Common.Interfaces.Repository;
using RentReady.DAL.Repository;

namespace RentReady.DAL
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
        {
            services.AddScoped<ITimeEntriesRepository, TimeEntriesRepository>();
            return services;
        }
        
    }
}