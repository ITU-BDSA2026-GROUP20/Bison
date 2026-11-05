using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Bison.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBisonInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Database>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}