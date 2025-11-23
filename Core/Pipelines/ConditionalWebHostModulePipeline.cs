using Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Pipelines;

internal sealed class ConditionalWebHostModulePipeline(IWebHostModulePipeline pipeline, bool condition)
    : WebHostModulePipelineBase, IConditionalWebHostModulePipeline
{
    private readonly bool _condition = condition;

    protected override IWebHostModulePipeline RootPipeline { get; } = pipeline;

    public override IWebHostModulePipeline AddModule(WebHostModuleBase module)
    {
        ArgumentNullException.ThrowIfNull(module);

        if (_condition)
        {
            RootPipeline.AddModule(module);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigureBuilder(Action<WebApplicationBuilder> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigureBuilder(configuration);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigureServices(Action<IServiceCollection> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigureServices(configuration);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigureServices(configuration);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration, IWebHostEnvironment> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigureServices(configuration);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigurePipeline(Action<WebApplication> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigurePipeline(configuration);
        }

        return RootPipeline;
    }

    public override IWebHostModulePipeline ConfigurePipeline(Func<WebApplication, Task> configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (_condition)
        {
            RootPipeline.ConfigurePipeline(configuration);
        }

        return RootPipeline;
    }
}