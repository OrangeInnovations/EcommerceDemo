using System;
using Microsoft.Extensions.Logging;

namespace CommonShares.ExceptionHandlers
{
    public interface INotifyMaintenance
    {
        void NotifyExceptionToDebugger(Exception exception, string classFunctionName = "");
        void LogAndNotifyToDebugger(Exception exception, string classFunctionName = "");

        void NotifyInfoToDebugger(string classFunctionName = "", string otherInfo = "");
        ILogger Logger { get; }
    }
}
