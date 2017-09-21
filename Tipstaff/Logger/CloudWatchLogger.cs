using System;
using System.Collections.Generic;

namespace Tipstaff.Logger
{
    public class CloudWatchLogger : ICloudWatchLogger
    {
        //private readonly TelemetryClient telemetryClient = new TelemetryClient();

        public void LogError(Exception exception, string message)
        {
            
        }

        //public void TrackTrace(string message, SeverityLevel level, IDictionary<string, string> properties)
        //{
        //    telemetryClient.TrackTrace(message, level, properties);
        //}
    }
}