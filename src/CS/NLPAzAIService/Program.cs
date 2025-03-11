using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using Azure;
using Azure.AI.TextAnalytics;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;
using System.Text;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

// 6. Custom Text Classification with Azure AI Service
// Get config settings from AppSettings
if (string.IsNullOrEmpty(appConfig.TranslatorServiceEndpoint) || string.IsNullOrEmpty(appConfig.TranslatorServiceKey) || string.IsNullOrEmpty(appConfig.TranslatorServiceRegion))
{
    WriteLine("Please check your appsettings.json file for missing or incorrect values.");
    return;
}

string translatorEndpoint = appConfig.TranslatorServiceEndpoint!;
string cogSvcKey = appConfig.TranslatorServiceKey!;
string cogSvcRegion = appConfig.TranslatorServiceRegion!;

// Set console encoding to unicode
Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

var folderPath = Path.GetFullPath(@"D:\STSA\learn-ai102-in-2025\src\Data\TextAnalysis\reviews");
DirectoryInfo folder = new(folderPath);

foreach (var file in folder.GetFiles("*.txt"))
{
    // Read the file contents
    Console.WriteLine("\n-------------\n" + file.Name);
    StreamReader sr = file.OpenText();
    var text = sr.ReadToEnd();
    sr.Close();
    Console.WriteLine("\n" + text);

    // Detect the language
    string language = await GetLanguage(text);
    Console.WriteLine("Language: " + language);

    // Translate if not already English
    if (language != "en")
    {
        string translatedText = await Translate(text, language);
        Console.WriteLine("\nTranslation:\n" + translatedText);
    }
}

static async Task<string> GetLanguage(string text)
{
    // Default language is English
    string language = "en";

    // Use the Azure AI Translator detect function


    // return the language
    return language;
}

static async Task<string> Translate(string text, string sourceLanguage)
{
    string translation = "";

    // Use the Azure AI Translator translate function


    // Return the translation
    return translation;

}
// 6. Custom Text Classification with Azure AI Service

// ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** ***** 

// Custom Text Classification with Azure AI Service
// Get config settings from AppSettings
if (string.IsNullOrEmpty(appConfig.CustomTextClassificationEndpoint) || string.IsNullOrEmpty(appConfig.CustomTextClassificationKey))
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
    AzureKeyCredential credentials = new(customTextClassificationKey);
    Uri endpoint = new(customTextClassificationEndpoint);
    TextAnalyticsClient textAnalyticsClient = new(endpoint, credentials);

    // Read each text file in the articles folder
    List<string> batchedDocuments = [];

    ForegroundColor = ConsoleColor.DarkCyan;

    WriteLine("***** This app uses the Azure Language Understanding service to analyze text input. *****");
    WriteLine("Reading text files from the articles folder...");
    var folderPath = Path.GetFullPath(@"D:\STSA\learn-ai102-in-2025\src\Data\TextAnalysis\articles");
    DirectoryInfo folder = new(folderPath);
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
    WriteLine("Classifying text files...");
    ClassifyDocumentOperation operation = await textAnalyticsClient.SingleLabelClassifyAsync(WaitUntil.Completed, batchedDocuments, projectName, deploymentName);

    int fileNo = 0;
    WriteLine("Results of Text Classification:");
    await foreach (ClassifyDocumentResultCollection documentsInPage in operation.Value)
    {

        foreach (ClassifyDocumentResult documentResult in documentsInPage)
        {
            Console.WriteLine(files[fileNo].Name);
            if (documentResult.HasError)
            {
                Console.WriteLine($"  Error!");
                Console.WriteLine($"  Document error code: {documentResult.Error.ErrorCode}");
                Console.WriteLine($"  Message: {documentResult.Error.Message}");
                continue;
            }

            Console.WriteLine($"  Predicted the following class:");
            Console.WriteLine();

            foreach (ClassificationCategory classification in documentResult.ClassificationCategories)
            {
                Console.WriteLine($"  Category: {classification.Category}");
                Console.WriteLine($"  Confidence score: {classification.ConfidenceScore}");
                Console.WriteLine();
            }
            fileNo++;
        }
    }
}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(ex.Message);
    throw;
}
finally
{
    ResetColor();
}
// Custom Language Understanding with Azure AI Service

await CLUWithAzureAIService.ShowCLUDemoWithAzureAIService(appConfig);

await QnAWithAzureAIService.ShowQnAWithAzureAIServiceDemo(appConfig);

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);