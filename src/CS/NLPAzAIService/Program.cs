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

// Custom Text Classification with Azure AI Service
// Get config settings from AppSettings
if (string.IsNullOrEmpty(appConfig.AiServicesEndpoint) || string.IsNullOrEmpty(appConfig.AiServicesKey))
{
    WriteLine("Please check your appsettings.json file for missing or incorrect values.");
    return;
}

string customTextClassificationEndpoint = appConfig.CustomTextClassificationEndpoint!;
string customTextClassificationKey = appConfig.CustomTextClassificationKey!;
string projectName = appConfig.CustomTextClassificationProjectName!;
string deploymentName = appConfig.CustomTextClassificationDeploymentName!;

try
{
    // Create client using endpoint and key


    // Read each text file in the articles folder
    List<string> batchedDocuments = new List<string>();

    var folderPath = Path.GetFullPath("./articles");
    DirectoryInfo folder = new DirectoryInfo(folderPath);
    FileInfo[] files = folder.GetFiles("*.txt");
    foreach (var file in files)
    {
        // Read the file contents
        StreamReader sr = file.OpenText();
        var text = sr.ReadToEnd();
        sr.Close();
        batchedDocuments.Add(text);
    }

    // Get Classifications
}
catch (Exception ex)
{
    WriteLine(ex.Message);
    throw;
}
// Custom Language Understanding with Azure AI Service

await CLUWithAzureAIService.ShowCLUDemoWithAzureAIService(appConfig);

await QnAWithAzureAIService.ShowQnAWithAzureAIServiceDemo(appConfig);

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);