using Microsoft.AspNetCore.Builder;

namespace Core.Pipelines;

public static class WebHostModulePipelineExtensions
{
    public static IWebHostModulePipeline ToPipeline(this WebApplicationBuilder appBuilder)
    {
        ArgumentNullException.ThrowIfNull(appBuilder);

        return new WebHostModulePipeline(appBuilder);
    }
}