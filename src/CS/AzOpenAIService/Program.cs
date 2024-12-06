using AzOpenAIService.Configuration;
using AzOpenAIService.Extensions;
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
bool printFullResponse = false;


// *************** Use your own data - RAG with Azure OpenAI Service ***************

// *************** Use your own data - RAG with Azure OpenAI Service ***************


// *************** Generate and improve code with Azure OpenAI Service ***************
string command;

do
{
    WriteLine("\n1: Add comments to my function\n" +
    "2: Write unit tests for my function\n" +
    "3: Fix my Go Fish game\n" +
    "\"quit\" to exit the program\n\n" +
    "Enter a number to select a task:");

    command = ReadLine() ?? "";

    if (command == "quit")
    {
        WriteLine("Exiting program...");
        break;
    }

    WriteLine("\nEnter a prompt: ");
    string userPrompt = ReadLine() ?? "";
    string codeFile = "";

    if (command == "1" || command == "2")
        codeFile = File.ReadAllText("D:\\STSA\\learn-ai-102-code\\src\\CS\\sample-code\\function\\function.cs");
    else if (command == "3")
        codeFile = File.ReadAllText("D:\\STSA\\learn-ai-102-code\\src\\CS\\sample-code\\go-fish\\go-fish.cs");
    else
    {
        WriteLine("Invalid input. Please try again.");
        continue;
    }

    userPrompt += codeFile;

    await GetResponseFromOpenAIForCodeGeneration(userPrompt);
} while (true);

async Task GetResponseFromOpenAIForCodeGeneration(string prompt)
{
    WriteLine("\nCalling Azure OpenAI to generate code...\n\n");

    if (string.IsNullOrEmpty(appConfig.AzureOpenAiEndpoint) || string.IsNullOrEmpty(appConfig.AzureOpenAiKey) || string.IsNullOrEmpty(appConfig.AzureOpenAiDeploymentName))
    {
        WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    // Configure the Azure OpenAI client
    OpenAIClient openAIClient = new(new Uri(appConfig.AzureOpenAiEndpoint), new AzureKeyCredential(appConfig.AzureOpenAiKey));

    // Define chat prompts
    string systemPrompt = "You are a helpful AI assistant that helps programmers write code.";
    string userPrompt = prompt;

    // Format and send the request to the model
    var chatCompletionsOptions = new ChatCompletionsOptions()
    {
        Messages =
     {
         new ChatRequestSystemMessage(systemPrompt),
         new ChatRequestUserMessage(userPrompt)
     },
        Temperature = 0.7f,
        MaxTokens = 1000,
        DeploymentName = appConfig.AzureOpenAiDeploymentName,
    };

    // Get response from Azure OpenAI
    Response<ChatCompletions> response = await openAIClient.GetChatCompletionsAsync(chatCompletionsOptions);

    ChatCompletions completions = response.Value;
    string completion = completions.Choices[0].Message.Content;

    // Write full response to console, if requested
    if (printFullResponse)
    {
        WriteLine($"\nFull response: {JsonSerializer.Serialize(completions, new JsonSerializerOptions { WriteIndented = true })}\n\n");
    }

    // Write the file.
    File.WriteAllText("D:\\STSA\\learn-ai-102-code\\src\\CS\\AzOpenAIService\\Results\\app.txt", completion);

    // Write response to console
    WriteLine($"\nResponse written to Results/app.txt\n\n");
}

// *************** Generate and improve code with Azure OpenAI Service ***************
