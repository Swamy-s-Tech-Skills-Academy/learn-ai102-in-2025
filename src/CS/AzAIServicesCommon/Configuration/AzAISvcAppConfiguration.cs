namespace AzAIServicesCommon.Configuration;

public class AzAISvcAppConfiguration
{
    public string? AiServicesEndpoint { get; set; }

    public string? AiServicesKey { get; set; }

    public string? AzureOpenAiEndpoint { get; set; }

    public string? AzureOpenAiKey { get; set; }

    public string? AzureOpenAiDeploymentName { get; set; }

    public string? AzureSearchEndpoint { get; set; }

    public string? AzureSearchKey { get; set; }

    public string? AzureSearchIndex { get; set; }

    public string? QAProjectName { get; set; }

    public string? QADeploymentName { get; set; }
}
