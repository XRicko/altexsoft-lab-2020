using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Infrastructure.Data;
using RecipeBook.SharedKernel.Interfaces;

namespace RecipeBook.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<RecipeBookContext>(options => options.UseSqlServer(connectionString)
                                                                 .UseLazyLoadingProxies());
            services.AddScoped<IRepository, EfRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
