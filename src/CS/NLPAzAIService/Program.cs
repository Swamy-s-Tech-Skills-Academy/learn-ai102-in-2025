using AzAIServicesCommon.Configuration;
using AzAIServicesCommon.Extensions;
using HeaderFooter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLPAzAIService.Services;
using Azure;
using Azure.AI.Language.Conversations;
using Azure.Core.Serialization;
using Azure.Core;
using System.Text.Json;

using IHost host = IHostExtensions.GetHostBuilder(args);

IHeader header = host.Services.GetRequiredService<IHeader>();
IFooter footer = host.Services.GetRequiredService<IFooter>();
AzAISvcAppConfiguration appConfig = host.Services.GetRequiredService<AzAISvcAppConfiguration>();

// ShowCLUDemoWithAzureAIService
try
{
    // Get config settings from AppSettings
    if (string.IsNullOrEmpty(appConfig.AiServicesEndpoint) || string.IsNullOrEmpty(appConfig.AiServicesKey))
    {
        WriteLine("Please check your appsettings.json file for missing or incorrect values.");
        return;
    }

    string predictionEndpoint = appConfig.AILanguageServiceEndpoint!;
    string predictionKey = appConfig.AILanguageServiceKey!;

    // Create a client for the Language service model
    Uri endpoint = new(predictionEndpoint);
    AzureKeyCredential credential = new(predictionKey);

    ConversationAnalysisClient client = new(endpoint, credential);

    // Get user input (until they enter "quit")
    string userText = "";
    while (userText!.ToLower() != "quit")
    {
        WriteLine("\nEnter some text ('quit' to stop)");
        userText = ReadLine()!;
        if (userText?.ToLower() != "quit")
        {

            // Call the Language service model to get intent and entities
            var projectName = "Clock";
            var deploymentName = "production";

            var data = new
            {
                analysisInput = new
                {
                    conversationItem = new
                    {
                        text = userText,
                        id = "1",
                        participantId = "1",
                    }
                },
                parameters = new
                {
                    projectName,
                    deploymentName,
                    // Use Utf16CodeUnit for strings in .NET.
                    stringIndexType = "Utf16CodeUnit",
                },
                kind = "Conversation",
            };

            // Send request
            Response response = await client.AnalyzeConversationAsync(RequestContent.Create(data));
            
            dynamic conversationalTaskResult = response.Content.ToDynamicFromJson(JsonPropertyNames.CamelCase);
            
            dynamic conversationPrediction = conversationalTaskResult.Result.Prediction;
            var options = new JsonSerializerOptions { WriteIndented = true };
            
            WriteLine(JsonSerializer.Serialize(conversationalTaskResult, options));
            WriteLine("--------------------\n");
            WriteLine(userText);
            var topIntent = "";
            if (conversationPrediction.Intents[0].ConfidenceScore > 0.5)
            {
                topIntent = conversationPrediction.TopIntent;
            }

            // Apply the appropriate action
            switch (topIntent)
            {
                case "GetTime":
                    var location = "local";
                    // Check for a location entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Location")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            location = entity.Text;
                        }
                    }
                    // Get the time for the specified location
                    string timeResponse = GetTime(location);
                    WriteLine(timeResponse);
                    break;
                case "GetDay":
                    var date = DateTime.Today.ToShortDateString();
                    // Check for a Date entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Date")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            date = entity.Text;
                        }
                    }
                    // Get the day for the specified date
                    string dayResponse = GetDay(date);
                    WriteLine(dayResponse);
                    break;
                case "GetDate":
                    var day = DateTime.Today.DayOfWeek.ToString();
                    // Check for entities            
                    // Check for a Weekday entity
                    foreach (dynamic entity in conversationPrediction.Entities)
                    {
                        if (entity.Category == "Weekday")
                        {
                            //Console.WriteLine($"Location Confidence: {entity.ConfidenceScore}");
                            day = entity.Text;
                        }
                    }
                    // Get the date for the specified day
                    string dateResponse = GetDate(day);
                    WriteLine(dateResponse);
                    break;
                default:
                    // Some other intent (for example, "None") was predicted
                    WriteLine("Try asking me for the time, the day, or the date.");
                    break;
            }

        }

    }

}
catch (Exception ex)
{
    ForegroundColor = ConsoleColor.Red;
    WriteLine(ex.Message);
}
finally
{
    ResetColor();
}
static string GetTime(string location)
{
    var timeString = "";
    var time = DateTime.Now;

    /* Note: To keep things simple, we'll ignore daylight savings time and support only a few cities.
       In a real app, you'd likely use a web service API (or write  more complex code!)
       Hopefully this simplified example is enough to get the the idea that you
       use LU to determine the intent and entities, then implement the appropriate logic */

    switch (location.ToLower())
    {
        case "local":
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "london":
            time = DateTime.UtcNow;
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "sydney":
            time = DateTime.UtcNow.AddHours(11);
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "new york":
            time = DateTime.UtcNow.AddHours(-5);
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "nairobi":
            time = DateTime.UtcNow.AddHours(3);
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "tokyo":
            time = DateTime.UtcNow.AddHours(9);
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        case "delhi":
            time = DateTime.UtcNow.AddHours(5.5);
            timeString = time.Hour.ToString() + ":" + time.Minute.ToString("D2");
            break;
        default:
            timeString = "I don't know what time it is in " + location;
            break;
    }

    return timeString;
}

static string GetDate(string day)
{
    string date_string = "I can only determine dates for today or named days of the week.";

    // To keep things simple, assume the named day is in the current week (Sunday to Saturday)
    DayOfWeek weekDay;
    if (Enum.TryParse(day, true, out weekDay))
    {
        int weekDayNum = (int)weekDay;
        int todayNum = (int)DateTime.Today.DayOfWeek;
        int offset = weekDayNum - todayNum;
        date_string = DateTime.Today.AddDays(offset).ToShortDateString();
    }
    return date_string;

}

static string GetDay(string date)
{
    // Note: To keep things simple, dates must be entered in US format (MM/DD/YYYY)
    string day_string = "Enter a date in MM/DD/YYYY format.";
    DateTime dateTime;
    if (DateTime.TryParse(date, out dateTime))
    {
        day_string = dateTime.DayOfWeek.ToString();
    }

    return day_string;
}
// ShowCLUDemoWithAzureAIService

await QnAWithAzureAIService.ShowQnAWithAzureAIServiceDemo(appConfig);

await TextAnalysisWithAzureAIService.ShowTextAnalysisWithAzureAIServiceDemo(appConfig);