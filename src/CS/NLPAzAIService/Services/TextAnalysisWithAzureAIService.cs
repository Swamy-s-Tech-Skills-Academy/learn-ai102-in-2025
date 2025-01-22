using AzAIServicesCommon.Configuration;
using Azure;
using Azure.AI.TextAnalytics;

namespace NLPAzAIService.Services;

internal sealed class TextAnalysisWithAzureAIService
{

    public static async Task ShowTextAnalysisWithAzureAIServiceDemo(AzAISvcAppConfiguration appConfig)
    {
        if (string.IsNullOrEmpty(appConfig.AiServicesEndpoint) || string.IsNullOrEmpty(appConfig.AiServicesKey))
        {
            WriteLine("Please check your appsettings.json file for missing or incorrect values.");
            return;
        }

        string aiSvcEndpoint = appConfig.AiServicesEndpoint!;
        string aiSvcKey = appConfig.AiServicesKey!;

        try
        {
            // Create client using endpoint and key
            AzureKeyCredential credentials = new(aiSvcKey);
            Uri endpoint = new(aiSvcEndpoint);
            TextAnalyticsClient textAnalyticsClient = new(endpoint, credentials);

            // Analyze each text file in the reviews folder
            var folderPath = Path.GetFullPath(@"D:\STSA\learn-ai102-in-2025\src\Data\TextAnalysis\reviews");
            DirectoryInfo folder = new(folderPath);

            foreach (var file in folder.GetFiles("*.txt"))
            {
                // Read the file contents
                ForegroundColor = ConsoleColor.White;
                WriteLine("\n-------------\n" + file.Name);
                StreamReader sr = file.OpenText();
                var text = await sr.ReadToEndAsync();
                sr.Close();
                WriteLine("\n" + text);

                // Get language
                ForegroundColor = ConsoleColor.DarkCyan;
                DetectedLanguage detectedLanguage = await textAnalyticsClient.DetectLanguageAsync(text);
                WriteLine($"\nLanguage: {detectedLanguage.Name}");

                // Get key phrases
                ForegroundColor = ConsoleColor.DarkYellow;
                KeyPhraseCollection phrases = await textAnalyticsClient.ExtractKeyPhrasesAsync(text);
                if (phrases.Count > 0)
                {
                    WriteLine("\nKey Phrases:");
                    foreach (string phrase in phrases)
                    {
                        WriteLine($"\t{phrase}");
                    }
                }

                // Get sentiment
                ForegroundColor = ConsoleColor.DarkGreen;
                DocumentSentiment sentimentAnalysis = await textAnalyticsClient.AnalyzeSentimentAsync(text);
                WriteLine($"\nSentiment: {sentimentAnalysis.Sentiment}");

                // Get entities
                ForegroundColor = ConsoleColor.Magenta;
                CategorizedEntityCollection entities = await textAnalyticsClient.RecognizeEntitiesAsync(text);
                if (entities.Count > 0)
                {
                    Console.WriteLine("\nEntities:");
                    foreach (CategorizedEntity entity in entities)
                    {
                        WriteLine($"\t{entity.Text} ({entity.Category})");
                    }
                }

                // Get linked entities
                ForegroundColor = ConsoleColor.Blue;
                LinkedEntityCollection linkedEntities = await textAnalyticsClient.RecognizeLinkedEntitiesAsync(text);
                if (linkedEntities.Count > 0)
                {
                    WriteLine("\nLinks:");
                    foreach (LinkedEntity linkedEntity in linkedEntities)
                    {
                        WriteLine($"\t{linkedEntity.Name} ({linkedEntity.Url})");
                    }
                }

                ResetColor();
            }
        }
        catch (Exception ex)
        {
            ResetColor();
            WriteLine(ex.Message);
        }
    }
}
