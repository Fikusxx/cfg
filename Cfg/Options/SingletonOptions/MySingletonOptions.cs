using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cfg.Options.SingletonOptions;

public static class DependencyInjection
{
    public static void AddSingletonOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MySingletonOptions>(builder.Configuration.GetSection(nameof(MySingletonOptions)))
            .ConfigureOptions<ConfigureMySingletonOptions>()
            .AddOptionsWithValidateOnStart<MySingletonOptions>()
            .ValidateDataAnnotations();

        // same result, more verbose api
        // builder.Services.AddOptions<MySingletonOptions>()
        //     .BindConfiguration(nameof(MySingletonOptions))
        //     .ValidateDataAnnotations()
        //     .ValidateOnStart();
        // builder.Services.ConfigureOptions<MySingletonOptions>();

        builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<MySingletonOptions>>().Value);
    }
}

public static class EndpointsExtensions
{
    public static void MapSingletonOptions(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/singleton", ([FromServices] MySingletonOptions options) =>
        {
            return Results.Ok(options);
        });
    }
}

public record MySingletonOptions
{
    [Length(1, 25)]
    public required string Name { get; set; }
}

public class ConfigureMySingletonOptions :
    IConfigureOptions<MySingletonOptions>,
    IValidateOptions<MySingletonOptions>,
    IPostConfigureOptions<MySingletonOptions>
{
    public void Configure(MySingletonOptions singletonOptions)
    {
        singletonOptions.Name = "IConfigureOptions";
    }

    public ValidateOptionsResult Validate(string? name, MySingletonOptions singletonOptions)
    {
        if (singletonOptions.Name.Length < 4)
            return ValidateOptionsResult.Fail("too short");

        return ValidateOptionsResult.Success;
    }

    public void PostConfigure(string? name, MySingletonOptions singletonOptions)
    {
        singletonOptions.Name = "IPostConfigureOptions";
    }
}