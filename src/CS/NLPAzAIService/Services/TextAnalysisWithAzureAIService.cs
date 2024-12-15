using AzAIServicesCommon.Configuration;

namespace NLPAzAIService.Services;

internal sealed class TextAnalysisWithAzureAIService
{

    public static async Task ShowTextAnalysisWithAzureAIServiceDemo(AzAISvcAppConfiguration appConfig)
    {
        if (string.IsNullOrEmpty(appConfig.AzureOpenAiEndpoint) || string.IsNullOrEmpty(appConfig.AzureOpenAiKey) || string.IsNullOrEmpty(appConfig.AzureOpenAiDeploymentName))
        {
            WriteLine("Please check your appsettings.json file for missing or incorrect values.");
            return;
        }

    }
}
