using Core.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Pipelines;

public interface IWebHostModulePipelineBase
{
    IWebHostModulePipeline AddModule(WebHostModuleBase module);
    IWebHostModulePipeline ConfigureBuilder(Action<WebApplicationBuilder> configuration);
    IWebHostModulePipeline ConfigureServices(Action<IServiceCollection> configuration);
    IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration> configuration);
    IWebHostModulePipeline ConfigureServices(Action<IServiceCollection, IConfiguration, IWebHostEnvironment> configuration);
    IWebHostModulePipeline ConfigurePipeline(Action<WebApplication> configuration);
    IWebHostModulePipeline ConfigurePipeline(Func<WebApplication, Task> configuration);
}