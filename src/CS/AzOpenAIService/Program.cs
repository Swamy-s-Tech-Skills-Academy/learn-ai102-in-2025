using AzOpenAIService.Configuration;
using AzOpenAIService.Extensions;
using Azure.AI.OpenAI;
using Azure;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

// Initialize the Azure OpenAI client
OpenAIClient openAIClient = new(new Uri(appConfig.AzureOpenAiEndpoint!), new AzureKeyCredential(appConfig.AzureOpenAiKey!));
