using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GTG_automation_tests.Classes
{
    internal class XMLRequest
    {
        public string CardEaseReference { get; set; }
        public string TerminalID { get; set; }
        public string TransactionKey { get; set; }

        public XMLRequest(string xmlPayload, string apiUrl)
        {
            // Load the XML payload into XDocument for parsing
            XDocument xmlDocRequest = XDocument.Parse(xmlPayload);

            // Extract TerminalID and TransactionKey
            TerminalID = xmlDocRequest.Root.Element("TerminalDetails")?.Element("TerminalID")?.Value;
            TransactionKey = xmlDocRequest.Root.Element("TerminalDetails")?.Element("TransactionKey")?.Value;

            if (TerminalID == null || TransactionKey == null)
            {
                throw new InvalidOperationException("TerminalID or TransactionKey is missing in the XML payload.");
            }

            Console.WriteLine("TerminalID: " + TerminalID);
            Console.WriteLine("TransactionKey: " + TransactionKey);

            // Make the API POST request
            MakeApiRequest(xmlPayload, apiUrl).Wait();
        }

        private async Task MakeApiRequest(string xmlPayload, string apiUrl)
        {
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
                CardEaseReference = xmlDoc.Root.Element("TransactionDetails")?.Value.Substring(0, 36);
                var localResult = xmlDoc.Root.Element("Result")?.Value.Substring(0, 1);
                var acquirerResponseCode = xmlDoc.Root.Element("Result")?.Value.Substring(1, 2);
                var cardDetails = xmlDoc.Root.Element("CardDetails")?.Value.Substring(113, 4);
                Console.WriteLine("CardEaseReference: " + CardEaseReference);
                Console.WriteLine("LocalResult: " + localResult);
                Console.WriteLine("AcquirerResponseCode: " + acquirerResponseCode);
                Console.WriteLine("CardDetails: " + cardDetails);
                // If the element exists, save it to a file or use it as needed
                if (CardEaseReference != null)
                {
                    // Save the extracted data to a file
                    File.WriteAllText("LocalResult.txt", CardEaseReference + "\n" + localResult + "\n" + acquirerResponseCode + "\n" + cardDetails);
                }
                else
                {
                    Console.WriteLine("No <LocalResult> element found in the XML response.");
                }
            }
        }
    }
}
