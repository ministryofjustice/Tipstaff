using System;

namespace Tipstaff.Logger
{
    public interface ICloudWatchLogger
    {
        void LogError(Exception exception, string message);

        //void TrackTrace(string message, SeverityLevel level, IDictionary<string, string> properties);
    }
}