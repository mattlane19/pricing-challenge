using Pricing_Challenge.Context;
using Pricing_Challenge.Interfaces;
using Pricing_Challenge.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Pricing_Challenge
{
    class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IEnvironment, EnvironmentService>();
            services.AddTransient<IConsole, ConsoleService>();
            services.AddTransient<IDataContext, DataContext>();
            services.AddTransient<IPricingService, PricingService>();
            services.AddTransient<IOffersService, OffersService>();
            services.AddTransient<IPriceBasketService, PriceBasketService>();
            services.AddTransient<CheckoutService>();
        }
    }
}
