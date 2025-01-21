using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;
// import namespaces
using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

await QnAWithAzureAIService.ShowQnAWithAzureAIServiceDemo(appConfig);

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);