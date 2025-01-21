using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);

// Question Answering Solution
try
{
    // Get config settings from AppSettings
    string aiSvcEndpoint = appConfig.AiServicesEndpoint!;
    string aiSvcKey = appConfig.AiServicesKey!;
    string projectName = appConfig.QAProjectName!;
    string deploymentName = appConfig.QADeploymentName!;

    // Create client using endpoint and key


    // Submit a question and display the answer


}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
// Question Answering Solution