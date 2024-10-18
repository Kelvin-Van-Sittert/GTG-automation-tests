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

string cardEaseReference = null;

//Create its own method
// API Endpoint URL
string apiUrl = StaticData.CETE_01_URL;
// XML payload to be posted
string xmlPayload = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>
                <Request type=""CardEaseXML"" version=""1.5.0"">
                    <TransactionDetails>
                        <MessageType autoconfirm=""true"">Auth</MessageType>
                        <Amount>3331</Amount>
                        <CurrencyCode>840</CurrencyCode>
                    </TransactionDetails>
                    <TerminalDetails>
                        <TerminalID>99962964</TerminalID>
                        <TransactionKey>4614</TransactionKey>
                    </TerminalDetails>
                    <CardDetails>
                        <Manual type=""mailorder"">
                            <PAN>4012000098765439</PAN>
                            <ExpiryDate format=""yyMM"">2512</ExpiryDate>
                        </Manual>
                        <AdditionalVerification>
                            <CSC>999</CSC>
                        </AdditionalVerification>
                    </CardDetails>
                </Request>";
// Make the API POST request
using (HttpClient client = new HttpClient())
{
    HttpContent content = new StringContent(xmlPayload, Encoding.UTF8, "application/xml");
    // Send the POST request
    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
    // Ensure the request was successful
    response.EnsureSuccessStatusCode();
    if (response.IsSuccessStatusCode)
    {
        // Read the response content
        string result = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Response: " + result);
    }
    else
    {
        Console.WriteLine("Error: " + response.StatusCode);
    }
    // Read the XML response content
    string xmlResponse = await response.Content.ReadAsStringAsync();
    // Load the XML response into XDocument for parsing
    XDocument xmlDoc = XDocument.Parse(xmlResponse);
    // Example: Extract the value of <ResponseData> element
    cardEaseReference = xmlDoc.Root.Element("TransactionDetails")?.Value.Substring(0, 36);
    var localResult = xmlDoc.Root.Element("Result")?.Value.Substring(0, 1);
    var acquirerResponseCode = xmlDoc.Root.Element("Result")?.Value.Substring(1, 2);
    var cardDetails = xmlDoc.Root.Element("CardDetails")?.Value.Substring(113, 4);
    Console.WriteLine("CardEaseReference: " + cardEaseReference);
    Console.WriteLine("LocalResult: " + localResult);
    Console.WriteLine("AcquirerResponseCode: " + acquirerResponseCode);
    Console.WriteLine("CardDetails: " + cardDetails);
    // If the element exists, save it to a file or use it as needed
    if (cardEaseReference != null)
    {
        // Save the extracted data to a file
        File.WriteAllText("LocalResult.txt", cardEaseReference + "\n" + localResult + "\n" + acquirerResponseCode + "\n" + cardDetails);
    }
    else
    {
        Console.WriteLine("No <LocalResult> element found in the XML response.");
    }
}

//Up to here

//Method for storing the log file
const string logFilePath = StaticData.TSYS_PROCESSOR;

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

    var responseOrRequest = "Response";

    Console.WriteLine("What is cardEaseReference: " + cardEaseReference);
    List<Log> matchingLogs = matchingLogs = logs.Where(log => log.Message.Contains(cardEaseReference)).ToList(); ;

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

            // Extract XML content from the message
            var xmlContent = ExtractXmlContent(log.Message);
            if (!string.IsNullOrEmpty(xmlContent))
            {
                Console.WriteLine("Extracted XML:");
                Console.WriteLine(xmlContent);
                /*
                                   // Read a specific attribute from the XML content
                                   var tagName = "Source";
                                   var attributeValue = ReadXmlAttribute(xmlContent, tagName);
                                   if (!string.IsNullOrEmpty(attributeValue))
                                   {
                                       Console.WriteLine($"Extracted Attribute Value: {attributeValue}");
                                   }
                                   else
                                   {
                                       Console.WriteLine("Attribute not found.");
                                   }
                */
            }
            else
            {
                Console.WriteLine("No XML content found.");
            }
        }
    }
    else
    {
        Console.WriteLine("No log entries found.");
    }
}

Console.WriteLine("Mission successful");



// Method to extract XML content from the message
string ExtractXmlContent(string message)
{
    var xmlPattern = @"<\?xml.*?\?>.*?</Request>";
    var match = System.Text.RegularExpressions.Regex.Match(message, xmlPattern, System.Text.RegularExpressions.RegexOptions.Singleline);
    return match.Success ? match.Value : null;
}

// Method to read a specific attribute from the XML content
string ReadXmlAttribute(string xmlContent, string tagName, string attributeName)
{
    var xDocument = XDocument.Parse(xmlContent);
    var element = xDocument.Descendants(tagName).FirstOrDefault();
    return element?.Attribute(attributeName)?.Value;
}