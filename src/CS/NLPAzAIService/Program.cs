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



// Question Answering Solution
try
{
    // Get config settings from AppSettings
    string aiSvcEndpoint = appConfig.AILanguageServiceEndpoint!;
    string aiSvcKey = appConfig.AILanguageServiceKey!;
    string projectName = appConfig.QAProjectName!;
    string deploymentName = appConfig.QADeploymentName!;

    if (string.IsNullOrEmpty(appConfig.AiServicesEndpoint) || string.IsNullOrEmpty(appConfig.AiServicesKey))
    {
        WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Create client using endpoint and key
    AzureKeyCredential credentials = new(aiSvcKey);
    Uri endpoint = new(aiSvcEndpoint);
    QuestionAnsweringClient aiClient = new(endpoint, credentials);

    // Submit a question and display the answer
    string user_question = "Hello";

    do
    {
        QuestionAnsweringProject project = new(projectName, deploymentName);
        Response<AnswersResult> response = aiClient.GetAnswers(user_question, project);

        foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
        {
            Console.WriteLine(answer.Answer);
            Console.WriteLine($"Confidence: {answer.Confidence:P2}");
            Console.WriteLine($"Source: {answer.Source}");
            Console.WriteLine();
        }

        Console.Write("Question: ");
        user_question = Console.ReadLine()!;

    } while (user_question?.ToLower() != "quit");

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
// Question Answering Solution

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);