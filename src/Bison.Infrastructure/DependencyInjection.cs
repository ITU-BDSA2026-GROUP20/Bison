using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Bison.Core.Repositories;
using Bison.Infrastructure.Repositories;

namespace Bison.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddBisonInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<Database>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<IObservationRepository, ObservationRepository>();
        services.AddScoped<IProposalRepository, ProposalRepository>();
        services.AddScoped<ITaxonRepository, TaxonRepository>();
        
        services.AddScoped<DatabaseInitializer>();

        return services;
    }
}