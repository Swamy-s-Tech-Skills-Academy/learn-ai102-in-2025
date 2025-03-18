﻿using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using Azure;
using Azure.AI.TextAnalytics;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;
using System.Text;
using Azure.AI.Translation.Text;

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
string translatorKey = appConfig.TranslatorServiceKey!;
string translatorRegion = appConfig.TranslatorServiceRegion!;

// Set console encoding to unicode
Console.InputEncoding = Encoding.Unicode;
Console.OutputEncoding = Encoding.Unicode;

// Create client using endpoint and key
AzureKeyCredential credential = new(translatorKey);
TextTranslationClient _textTranslationClient = new(credential, translatorRegion);

// Choose target language
Response<GetLanguagesResult> languagesResponse = await _textTranslationClient.GetLanguagesAsync(scope: "translation").ConfigureAwait(false);
GetLanguagesResult languages = languagesResponse.Value;
Console.WriteLine($"{languages.Translation.Count} languages available.\n(See https://learn.microsoft.com/azure/ai-services/translator/language-support#translation)");
Console.WriteLine("Enter a target language code for translation (for example, 'en'):");
string targetLanguage = "xx";
bool languageSupported = false;

while (!languageSupported)
{
    targetLanguage = Console.ReadLine();
    if (languages.Translation.ContainsKey(targetLanguage))
    {
        languageSupported = true;
    }
    else
    {
        Console.WriteLine($"{targetLanguage} is not a supported language.");
    }

}


// Translate text
string inputText = "";
while (inputText.ToLower() != "quit")
{
    Console.WriteLine("Enter text to translate ('quit' to exit)");
    inputText = Console.ReadLine();
    if (inputText.ToLower() != "quit")
    {
        Response<IReadOnlyList<TranslatedTextItem>> translationResponse = await _textTranslationClient.TranslateAsync(targetLanguage, inputText).ConfigureAwait(false);
        IReadOnlyList<TranslatedTextItem> translations = translationResponse.Value;
        TranslatedTextItem translation = translations[0];
        string sourceLanguage = translation?.DetectedLanguage?.Language;
        Console.WriteLine($"'{inputText}' translated from {sourceLanguage} to {translation?.Translations[0].To} as '{translation?.Translations?[0]?.Text}'.");
    }
}

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

async Task<string> GetLanguage(string text)
{
    // Default language is English
    string language = "en";

    // Use the Azure AI Translator detect function
    Response<IReadOnlyList<TranslatedTextItem>> translationResponse = await _textTranslationClient.TranslateAsync("nl", text).ConfigureAwait(false);
    IReadOnlyList<TranslatedTextItem> translations = translationResponse.Value;
    TranslatedTextItem translation = translations[0];
    language = translation?.DetectedLanguage?.Language;

    // return the language
    return language;
}

async Task<string> Translate(string text, string sourceLanguage)
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
//if (string.IsNullOrEmpty(appConfig.CustomTextClassificationEndpoint) || string.IsNullOrEmpty(appConfig.CustomTextClassificationKey))
//{
//    WriteLine("Please check your appsettings.json file for missing or incorrect values.");
//    return;
//}

//string customTextClassificationEndpoint = appConfig.CustomTextClassificationEndpoint!;
//string customTextClassificationKey = appConfig.CustomTextClassificationKey!;
//string projectName = appConfig.CustomTextClassificationProjectName!;
//string deploymentName = appConfig.CustomTextClassificationDeploymentName!;

//try
//{
//    // Create client using endpoint and key
//    AzureKeyCredential credentials = new(customTextClassificationKey);
//    Uri endpoint = new(customTextClassificationEndpoint);
//    TextAnalyticsClient textAnalyticsClient = new(endpoint, credentials);

//    // Read each text file in the articles folder
//    List<string> batchedDocuments = [];

//    ForegroundColor = ConsoleColor.DarkCyan;

//    WriteLine("***** This app uses the Azure Language Understanding service to analyze text input. *****");
//    WriteLine("Reading text files from the articles folder...");
//    var folderPath = Path.GetFullPath(@"D:\STSA\learn-ai102-in-2025\src\Data\TextAnalysis\articles");
//    DirectoryInfo folder = new(folderPath);
//    FileInfo[] files = folder.GetFiles("*.txt");
//    foreach (var file in files)
//    {
//        // Read the file contents
//        StreamReader sr = file.OpenText();
//        var text = sr.ReadToEnd();
//        sr.Close();
//        batchedDocuments.Add(text);
//    }

//    // Get Classifications
//    WriteLine("Classifying text files...");
//    ClassifyDocumentOperation operation = await textAnalyticsClient.SingleLabelClassifyAsync(WaitUntil.Completed, batchedDocuments, projectName, deploymentName);

//    int fileNo = 0;
//    WriteLine("Results of Text Classification:");
//    await foreach (ClassifyDocumentResultCollection documentsInPage in operation.Value)
//    {

//        foreach (ClassifyDocumentResult documentResult in documentsInPage)
//        {
//            Console.WriteLine(files[fileNo].Name);
//            if (documentResult.HasError)
//            {
//                Console.WriteLine($"  Error!");
//                Console.WriteLine($"  Document error code: {documentResult.Error.ErrorCode}");
//                Console.WriteLine($"  Message: {documentResult.Error.Message}");
//                continue;
//            }

//            Console.WriteLine($"  Predicted the following class:");
//            Console.WriteLine();

//            foreach (ClassificationCategory classification in documentResult.ClassificationCategories)
//            {
//                Console.WriteLine($"  Category: {classification.Category}");
//                Console.WriteLine($"  Confidence score: {classification.ConfidenceScore}");
//                Console.WriteLine();
//            }
//            fileNo++;
//        }
//    }
//}
//catch (Exception ex)
//{
//    ForegroundColor = ConsoleColor.Red;
//    WriteLine(ex.Message);
//    throw;
//}
//finally
//{
//    ResetColor();
//}
// Custom Language Understanding with Azure AI Service

await CLUWithAzureAIService.ShowCLUDemoWithAzureAIService(appConfig);

await QnAWithAzureAIService.ShowQnAWithAzureAIServiceDemo(appConfig);

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);