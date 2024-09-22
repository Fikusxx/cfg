using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Cfg.Options.ScopedOptions;

public static class DependencyInjection
{
    public static void AddScopedOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<MyScopedOptions>(builder.Configuration.GetSection(nameof(MyScopedOptions)))
            .ConfigureOptions<ConfigureMyScopedOptions>()
            .AddOptionsWithValidateOnStart<MyScopedOptions>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IOptionsSnapshot<MyScopedOptions>>().Value);

        builder.Services.AddHostedService<ScopedOptionsNotificationService>();
    }
}

public static class EndpointsExtensions
{
    public static void MapScopedOptions(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/scoped", (
            [FromServices] MyScopedOptions options,
            [FromServices] IServiceProvider sp) =>
        {
            var spOptions = sp.GetRequiredService<MyScopedOptions>();
            return Results.Ok(new { options, spOptions });
        });
    }
}

public record MyScopedOptions
{
    public required string Value { get; set; }
}

public class ConfigureMyScopedOptions :
    IConfigureOptions<MyScopedOptions>,
    IValidateOptions<MyScopedOptions>,
    IPostConfigureOptions<MyScopedOptions>
{
    // potential di

    /// <summary>
    /// can override values
    /// </summary>
    public void Configure(MyScopedOptions options)
    {
        // options.Value = "IConfigureOptions";
    }

    /// <summary>
    /// transient validation
    /// </summary>
    public ValidateOptionsResult Validate(string? name, MyScopedOptions options)
    {
        if (options.Value.Length < 4)
            return ValidateOptionsResult.Fail("too short");

        return ValidateOptionsResult.Success;
    }

    /// <summary>
    /// can override values
    /// </summary>
    public void PostConfigure(string? name, MyScopedOptions options)
    {
        // options.Value = "IPostConfigureOptions";
    }
}