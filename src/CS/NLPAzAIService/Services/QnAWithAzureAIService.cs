using AzAIServicesCommon.Configuration;
using Azure;
using Azure.AI.Language.QuestionAnswering;

namespace NLPAzAIService.Services;

internal sealed class QnAWithAzureAIService
{

    public static async Task ShowQnAWithAzureAIServiceDemo(AzAISvcAppConfiguration appConfig)
    {
        if (string.IsNullOrEmpty(appConfig.AILanguageServiceEndpoint) ||
            string.IsNullOrEmpty(appConfig.AILanguageServiceKey) ||
            string.IsNullOrEmpty(appConfig.QAProjectName) ||
            string.IsNullOrEmpty(appConfig.QADeploymentName))
        {
            WriteLine("Please check your appsettings.json file for missing or incorrect values.");
            return;
        }

        // Get config settings from AppSettings
        string aiSvcEndpoint = appConfig.AILanguageServiceEndpoint!;
        string aiSvcKey = appConfig.AILanguageServiceKey!;
        string projectName = appConfig.QAProjectName!;
        string deploymentName = appConfig.QADeploymentName!;

        ForegroundColor = ConsoleColor.DarkYellow;

        try
        {
            // Create client using endpoint and key
            AzureKeyCredential credentials = new(aiSvcKey);
            Uri endpoint = new(aiSvcEndpoint);
            QuestionAnsweringClient aiClient = new(endpoint, credentials);

            // Submit a question and display the answer
            string user_question = "Hello";

            do
            {
                QuestionAnsweringProject project = new(projectName, deploymentName);
                Response<AnswersResult> response = await aiClient.GetAnswersAsync(user_question, project);

                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                {
                    WriteLine(answer.Answer);
                    WriteLine($"Confidence: {answer.Confidence:P2}");
                    WriteLine($"Source: {answer.Source}");
                    WriteLine();
                }

                Write("Question: ");
                user_question = ReadLine()!;

            } while (user_question?.ToLower() != "quit");

        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
        }
        finally
        {
            ResetColor();
        }
    }
}
