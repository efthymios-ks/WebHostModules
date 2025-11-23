using Microsoft.AspNetCore.Builder;

namespace Core.Modules;

public abstract class WebHostModuleBase
{
    public virtual void ConfigureAppBuilder(WebApplicationBuilder appBuilder)
    {
    }

    public virtual Task ConfigureAppAsync(WebApplication app)
        => Task.CompletedTask;

    public virtual IEnumerable<Type> Dependencies { get; } = [];
}
