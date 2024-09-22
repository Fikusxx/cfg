using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cfg.Options.NamedOptions;

public static class DependencyInjection
{
    public static void AddNamedOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MyNamedOptions>(name: "One",
            builder.Configuration.GetSection($"{nameof(MyNamedOptions)}:One"));
        builder.Services.Configure<MyNamedOptions>(name: "Two",
            builder.Configuration.GetSection($"{nameof(MyNamedOptions)}:Two"));
        
        // builder.Services
        //     .AddOptions<MyNamedOptions>(name: null)
        //     // Configure<T1, T2, T3> can be some services
        //     .Configure<MyNamedOptions, MyNamedOptions, MyNamedOptions>((o, s1, s2, s3) =>
        // {
        //     // o.Name = s1.Method() + s2.Method()..
        // });
    }
}

public static class EndpointsExtensions
{
    public static void MapNamedOptions(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/named", ([FromServices] IOptionsSnapshot<MyNamedOptions> options) =>
        {
            var one = options.Get("One");
            var two = options.Get("Two");
            
            return Results.Ok(new { one, two });
        });
    }
}

public class MyNamedOptions
{
    public required string Name { get; set; }
}