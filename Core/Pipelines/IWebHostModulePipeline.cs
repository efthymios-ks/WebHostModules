using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Core.Pipelines;

public interface IWebHostModulePipeline : IWebHostModulePipelineBase
{
    IConditionalWebHostModulePipeline When(bool condition);
    IConditionalWebHostModulePipeline When(Func<bool> conditionFunc);
    IConditionalWebHostModulePipeline When(Func<IConfiguration, bool> conditionFunc);
    IConditionalWebHostModulePipeline WhenEnvironment(string environmentName);
    Task<WebApplication> BuildAsync();
}