using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);