using AzOpenAIService.Configuration;
using AzOpenAIService.Extensions;
using AzOpenAIService.Services;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

await GenerateCodeWithAzureOpenAIService.ShowGenerateCodeWithAzureOpenAIDemo(appConfig);

await RAGWithAzureOpenAIService.ShowRAGWithAzureOpenAIDemo(appConfig);
