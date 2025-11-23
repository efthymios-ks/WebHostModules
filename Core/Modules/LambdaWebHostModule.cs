using Microsoft.AspNetCore.Builder;

namespace Core.Modules;

internal sealed class LambdaWebHostModule(
    Action<WebApplicationBuilder> appBuilderConfigure,
    Func<WebApplication, Task> appConfiguration
    ) : WebHostModuleBase
{
    private static readonly Action<WebApplicationBuilder> _noOpAppBuilder = _ => { };
    private static readonly Func<WebApplication, Task> _noOpApp = _ => Task.CompletedTask;

    private readonly Action<WebApplicationBuilder> _configureBuilder = appBuilderConfigure ?? _noOpAppBuilder;
    private readonly Func<WebApplication, Task> _configureApp = appConfiguration ?? _noOpApp;

    public LambdaWebHostModule(Action<WebApplicationBuilder> configuration)
        : this(configuration, _noOpApp)
    {
    }

    public LambdaWebHostModule(Func<WebApplication, Task> configuration)
        : this(_noOpAppBuilder, configuration)
    {
    }

    public override void ConfigureAppBuilder(WebApplicationBuilder appBuilder)
        => _configureBuilder(appBuilder);

    public override async Task ConfigureAppAsync(WebApplication app)
        => await _configureApp(app);
}
