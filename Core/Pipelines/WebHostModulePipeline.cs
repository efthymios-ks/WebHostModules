using Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Pipelines;

internal sealed class WebHostModulePipeline : WebHostModulePipelineBase, IWebHostModulePipeline
{
    private readonly WebApplicationBuilder _appBuilder;
    private readonly ICollection<WebHostModuleBase> _modules = [];

    protected override IWebHostModulePipeline RootPipeline => this;

    internal WebHostModulePipeline(WebApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        _appBuilder = appBuilder;
    }

    public override IWebHostModulePipeline AddModule(WebHostModuleBase module)
    {
        ArgumentNullException.ThrowIfNull(module);

        _modules.Add(module);
        return this;
    }

    public IConditionalWebHostModulePipeline When(bool condition)
        => new ConditionalWebHostModulePipeline(this, condition);

    public IConditionalWebHostModulePipeline When(Func<bool> conditionFunc)
    {
        ArgumentNullException.ThrowIfNull(conditionFunc);

        return When(conditionFunc());
    }

    public IConditionalWebHostModulePipeline When(Func<IConfiguration, bool> conditionFunc)
    {
        ArgumentNullException.ThrowIfNull(conditionFunc);

        return When(() => conditionFunc(_appBuilder.Configuration));
    }

    public IConditionalWebHostModulePipeline WhenEnvironment(string environmentName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(environmentName);

        return When(() => string.Equals(_appBuilder.Environment.EnvironmentName, environmentName));
    }

    public async Task<WebApplication> BuildAsync()
    {
        foreach (var module in _modules)
        {
            module.ConfigureAppBuilder(_appBuilder);
        }

        var app = _appBuilder.Build();
        var registeredModuleTypes = new HashSet<Type>(_modules.Select(module => module.GetType()));
        foreach (var module in _modules)
        {
            foreach (var dependency in module.Dependencies)
            {
                var isServiceRegistered = app.Services.GetService(dependency) is not null;
                var isModuleRegistered = registeredModuleTypes.Contains(dependency);

                if (!isServiceRegistered && !isModuleRegistered)
                {
                    throw new InvalidOperationException(
                        $"The module '{module.GetType().FullName}' has an unfulfilled dependency on '{dependency.FullName}'.");
                }
            }

            await module.ConfigureAppAsync(app);
        }

        return app;
    }
}
