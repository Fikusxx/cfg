using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cfg.Options.DictionaryOptions;

public static class DependencyInjection
{
    public static void AddDictionaryOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MyDictionaryOptions>(builder.Configuration.GetSection(nameof(MyDictionaryOptions)));
    }
}

public static class EndpointsExtensions
{
    public static void MapDictionaryOptions(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/dictionary", (
            [FromServices] IOptions<MyDictionaryOptions> options) =>
        {
            return Results.Ok(options.Value);
        });
    }
}

public class MyDictionaryOptions
{
    public Dictionary<Priority, ConsumerGroupOptions> Dictionary { get; set; }
}

public enum Priority
{
    One,
    Two
}

public class ConsumerGroupOptions
{
    public required string Topic { get; set; }
    public required string Group { get; set; }
}