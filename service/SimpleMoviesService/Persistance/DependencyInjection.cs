using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;
using System.Reflection;

namespace SimpleMoviesService.Persistance;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IMoviesDbContext, MoviesDbContext>(options =>
                   options.UseSqlServer(configuration.GetConnectionString("MoviesDb")));

        using var scope = services.BuildServiceProvider().CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MoviesDbContext>();

        context.Database.Migrate();
        context.SeedData();

        return services;

    }
}
