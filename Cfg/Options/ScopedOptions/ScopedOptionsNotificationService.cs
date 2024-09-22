using Microsoft.Extensions.Options;

namespace Cfg.Options.ScopedOptions;

public sealed class ScopedOptionsNotificationService : BackgroundService
{
    private readonly IOptionsMonitor<MyScopedOptions> options;
    private readonly IDisposable? subscriber;

    public ScopedOptionsNotificationService(IOptionsMonitor<MyScopedOptions> options)
    {
        this.options = options;
        this.subscriber = this.options.OnChange(newOptions =>
        {
            Console.WriteLine($"Value changed to {newOptions.Value}");
        });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (stoppingToken.IsCancellationRequested == false)
        {
            await Task.Delay(1, stoppingToken);
        }
    }

    public override void Dispose()
    {
        subscriber?.Dispose();
    }
}