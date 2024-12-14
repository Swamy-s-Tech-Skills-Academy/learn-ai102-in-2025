using AzOpenAIService.Configuration;
using Azure;
using Azure.AI.OpenAI;

namespace AzOpenAIService.Services;

internal static class RAGWithAzureOpenAIService
{
    public static async Task ShowRAGWithAzureOpenAIDemo(AzAISvcAppConfiguration appConfig)
    {
        // Flag to show citations
        bool showCitations = true;

        if (string.IsNullOrEmpty(appConfig.AzureOpenAiEndpoint) || string.IsNullOrEmpty(appConfig.AzureOpenAiKey) || string.IsNullOrEmpty(appConfig.AzureOpenAiDeploymentName))
        {
            WriteLine("Please check your appsettings.json file for missing or incorrect values.");
            return;
        }

        // Configure the Azure OpenAI client
        OpenAIClient openAIClient = new(new Uri(appConfig.AzureOpenAiEndpoint), new AzureKeyCredential(appConfig.AzureOpenAiKey));

        // Get the prompt text
        WriteLine("Enter a question:");
        string inputPrompt = ReadLine() ?? "";

        // Configure your data source
        AzureSearchChatExtensionConfiguration ownDataConfig = new()
        {
            SearchEndpoint = new Uri(appConfig.AzureSearchEndpoint!),
            Authentication = new OnYourDataApiKeyAuthenticationOptions(appConfig.AzureSearchKey),
            IndexName = appConfig.AzureSearchIndex
        };

        // Send request to Azure OpenAI model  
        WriteLine("...Sending the following request to Azure OpenAI endpoint...");
        WriteLine("Request: " + inputPrompt + "\n");

        const string systemMessage = "You are a helpful assistant assisting users with travel recommendations.";

        ChatCompletionsOptions chatCompletionsOptions = new()
        {
            Messages =
        {
            new ChatRequestSystemMessage(systemMessage),
            new ChatRequestUserMessage(inputPrompt)
        },
            MaxTokens = 600,
            Temperature = 0.9f,
            DeploymentName = appConfig.AzureOpenAiDeploymentName,
            // Specify extension options
            AzureExtensionsOptions = new AzureChatExtensionsOptions()
            {
                Extensions = { ownDataConfig }
            }
        };

        ChatCompletions response = await openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);
        ChatResponseMessage responseMessage = response.Choices[0].Message;

        // Print response
        WriteLine("Response: " + responseMessage.Content + "\n");
        WriteLine("  Intent: " + responseMessage.AzureExtensionsContext?.Intent);

        if (showCitations)
        {
            WriteLine($"\n  Citations of data used:");

            if (responseMessage.AzureExtensionsContext is not null)
            {
                foreach (AzureChatExtensionDataSourceResponseCitation citation in responseMessage.AzureExtensionsContext.Citations)
                {
                    WriteLine($"    Citation: {citation.Title} - {citation.Url}");
                }
            }
        }

    }
}
