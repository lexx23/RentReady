using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
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

            var userName = Environment.GetEnvironmentVariable("Dataverse_UserName", EnvironmentVariableTarget.Process);
            var password = Environment.GetEnvironmentVariable("Dataverse_Password", EnvironmentVariableTarget.Process);
            var environment = Environment.GetEnvironmentVariable("Dataverse_Environment", EnvironmentVariableTarget.Process);

            var dataverseSettings = new Dataverse
            {
                UserName = userName,
                Password = password,
                Environment = environment
            };

            builder.Services.AddSingleton(dataverseSettings);
        }
    }
}