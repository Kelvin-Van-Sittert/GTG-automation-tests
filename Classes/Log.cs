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
        public static List<Log> GetMatchingRequestLogs(List<Log> logs, string ID, string whatToCheck)
        {
            List<Log> matchingLogs;
           
            return matchingLogs = logs.Where(log => log.Message.Contains(ID) && log.Message.Contains(whatToCheck) && log.Message.Contains("->")).ToList();
        }
        public static List<Log> GetMatchingResponseLogs(List<Log> logs, string ID, string whatToCheck)
        {
            List<Log> matchingLogs;

            return matchingLogs = logs.Where(log => log.Message.Contains(ID) && log.Message.Contains(whatToCheck) && log.Message.Contains("<-")).ToList();
        }

    }
}
