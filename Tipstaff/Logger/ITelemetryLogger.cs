using System;

namespace Tipstaff.Logger
{
    public interface ITelemetryLogger
    {
        void LogError(Exception exception, string message);

        //void TrackTrace(string message, SeverityLevel level, IDictionary<string, string> properties);
    }
}