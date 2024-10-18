using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTG_automation_tests.Objects
{
    internal class Log
    {
        public string Date { get; set; }
        public string Timestamp { get; set; }
        public string Information { get; set; }
        public string Message { get; set; }

        // Constructor that requires date, timestamp, information, and message
        public Log(string date, string timestamp, string information, string message)
        {
            Date = date;
            Timestamp = timestamp;
            Information = information;
            Message = message;
        }
        public static List<Log> GetMatchingLogs(List<Log> logs, string ID, string responseOrRequest)
        {
            List<Log> matchingLogs;

            if (responseOrRequest == "Request")
            {
                return matchingLogs = logs.Where(log => log.Message.Contains("43a62e98-928c-ef11-8aab-005056b9f3c3") && log.Message.Contains(responseOrRequest) && log.Message.Contains("xml") && !log.Message.Contains("Response")).ToList();
            }
            else
            {
                return matchingLogs = logs.Where(log => log.Message.Contains("43a62e98-928c-ef11-8aab-005056b9f3c3") && log.Message.Contains(responseOrRequest) && log.Message.Contains("xml") && !log.Message.Contains("Request")).ToList();
            }
            

        }

    }
}
