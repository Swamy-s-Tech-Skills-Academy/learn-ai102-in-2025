using AzOpenAIService.Configuration;
using AzOpenAIService.Extensions;
using AzOpenAIService.Services;
using Azure;
using Azure.AI.OpenAI;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

//await RAGWithAzureOpenAIService.ShowRAGWithAzureOpenAIDemo(appConfig);

await GenerateCodeWithAzureOpenAIService.ShowGenerateCodeWithAzureOpenAIDemo(appConfig);

