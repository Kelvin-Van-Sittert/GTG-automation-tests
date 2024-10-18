using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Linq;
using System.Buffers;
using GTG_automation_tests.Objects;
using GTG_automation_tests.Utilities;
using GTG_automation_tests.Classes;

XMLRequest xmlRequest = new XMLRequest(StaticData.xmlPayloadForTesting3DSWithMasterCard, StaticData.CETE_01_URL);

//Method for storing the log file
const string logFilePath = StaticData.TSYS_PROCESSOR_LOG_PATH;

using (FileStream fs = new FileStream(logFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
{
    String logContent = await reader.ReadToEndAsync();

    List<Log> logs = new List<Log>();

    //Storing the log file content in a list
    foreach (string val in logContent.Split('\n'))
    {
        string[] segments = val.Split(new[] { ' ' }, 4, StringSplitOptions.None);
        if (segments.Length == 4)
        {
            string date = segments[0];
            string timestamp = segments[1];
            string information = segments[2];
            string message = segments[3];

            logs.Add(new Log(date, timestamp, information, message));
        }
    }

    // Find log objects contained within the logs list that have a matching message value equal to the search value".

    Console.WriteLine("What is cardEaseReference: " + xmlRequest.CardEaseReference);
    string whatToCheck = "";
    //for the response
    //string whatToCheck = "[GS]020[GS]";
    //for the request
    //string whatToCheck = "[FS]000000003331[FS]";

    Console.WriteLine("What is whatToCheck: " + whatToCheck);

    //List<Log> matchingLogs = matchingLogs = Log.GetMatchingRequestLogs(logs, cardEaseReference, whatToCheck);
    List<Log> matchingLogs = matchingLogs = Log.GetMatchingResponseLogs(logs, xmlRequest.CardEaseReference, whatToCheck);

    if (matchingLogs != null && matchingLogs.Count != 0)
    {
        Console.WriteLine("Found log entries:");
        foreach (var log in matchingLogs)
        {
            Console.WriteLine($"Date: {log.Date}");
            Console.WriteLine($"Timestamp: {log.Timestamp}");
            Console.WriteLine($"Information: {log.Information}");
            Console.WriteLine($"Message: {log.Message}");
            Console.WriteLine();
        }
    }
    else
    {
        Console.WriteLine("No log entries found.");
    }
}

Console.WriteLine("Mission successful");