using AzAIServicesCommon.Configuration;

namespace NLPAzAIService.Services;

internal sealed class TextAnalysisWithAzureAIService
{

    public static async Task ShowTextAnalysisWithAzureAIServiceDemo(AzAISvcAppConfiguration appConfig)
    {
        string aiSvcEndpoint = appConfig.AiServicesEndpoint;
        string aiSvcKey = appConfig.AiServicesKey;

        if (string.IsNullOrEmpty(appConfig.AiServicesEndpoint) || string.IsNullOrEmpty(appConfig.AiServicesKey))
        {
            WriteLine("Please check your appsettings.json file for missing or incorrect values.");
            return;
        }

        try
        {
            // Create client using endpoint and key


            // Analyze each text file in the reviews folder
            var folderPath = Path.GetFullPath("./reviews");
            DirectoryInfo folder = new DirectoryInfo(folderPath);
            foreach (var file in folder.GetFiles("*.txt"))
            {
                // Read the file contents
                Console.WriteLine("\n-------------\n" + file.Name);
                StreamReader sr = file.OpenText();
                var text = sr.ReadToEnd();
                sr.Close();
                Console.WriteLine("\n" + text);

                // Get language


                // Get sentiment


                // Get key phrases


                // Get entities


                // Get linked entities

            }
        }
        catch (Exception ex)
        {
            WriteLine(ex.Message);
        }
    }
}
