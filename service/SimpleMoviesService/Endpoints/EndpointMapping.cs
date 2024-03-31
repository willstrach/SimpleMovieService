using SimpleMoviesService.Models;
using System.Reflection;

namespace SimpleMoviesService.Endpoints;

public static class EndpointMapping
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var endpointMapperTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.GetInterfaces().Contains(typeof(IEndpoint)));

        foreach (var endpointMapperType in endpointMapperTypes)
        {
            var endpointMapper = (IEndpoint)Activator.CreateInstance(endpointMapperType)!;
            endpointMapper.Map(app);
        }
    }
}

public interface IEndpoint
{
    void Map(IEndpointRouteBuilder app);
}