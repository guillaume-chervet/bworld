using System;
using Microsoft.Extensions.Logging;

namespace Demo.Log
{
    public class GetLogsInput
    {
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LogLevel? Level { get; set; }
        public string Filter { get; set; }
        public string Origin { get; set; }
        public int? Limit { get; set; }
    }
}