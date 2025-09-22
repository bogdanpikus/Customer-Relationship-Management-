using CRM.Models;
using CRM.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CRM.Services
{
    public static class DependencyInjectionConfig //For DI pattern
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<DuckDatabase>(provider => new DuckDatabase());

            services.AddTransient<OrderControlViewModel>();
            services.AddTransient<OrderModalViewModel>();
            services.AddTransient<CreateDatabaseViewModel>();

            services.AddTransient<DialogService>();

            return services.BuildServiceProvider();
        }
    }
}
