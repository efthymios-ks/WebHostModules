using Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Pipelines;

internal abstract class WebHostModulePipelineBase : IWebHostModulePipelineBase
{
    protected abstract IWebHostModulePipeline RootPipeline { get; }

    public abstract IWebHostModulePipeline AddModule(WebHostModuleBase module);

    public virtual IWebHostModulePipeline ConfigureBuilder(Action<WebApplicationBuilder> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return AddModule(new LambdaWebHostModule(configuration));
    }

    public virtual IWebHostModulePipeline ConfigureServices(Action<IServiceCollection> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return ConfigureBuilder(appBuilder
            => configuration(appBuilder.Services)
        );
    }

    public virtual IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return ConfigureBuilder(appBuilder
            => configuration(appBuilder.Services, appBuilder.Configuration)
        );
    }

    public virtual IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration, IWebHostEnvironment> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return ConfigureBuilder(appBuilder
            => configuration(appBuilder.Services, appBuilder.Configuration, appBuilder.Environment)
        );
    }

    public virtual IWebHostModulePipeline ConfigurePipeline(Action<WebApplication> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return ConfigurePipeline(app =>
        {
            configuration(app);
            return Task.CompletedTask;
        });
    }

    public virtual IWebHostModulePipeline ConfigurePipeline(Func<WebApplication, Task> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        return AddModule(new LambdaWebHostModule(configuration));
    }
}