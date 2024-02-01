using System;
using Pricing_Challenge.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Pricing_Challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();

            Startup startup = new();
            startup.ConfigureServices(services);

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            // Run the console display via the CheckoutService
            serviceProvider.GetService<CheckoutService>().Run();
        }

    }
}
