using System;
using CommonShares.EmailSend;
using CommonShares.Extentions;
using Microsoft.Extensions.Logging;

namespace CommonShares.ExceptionHandlers
{
    public class NotifyMaintenance : INotifyMaintenance
    {
        private readonly IEmailProvider _emailProvider;
        private readonly BaseEmailSettings _baseSettings;
        private readonly ILogger _logger;
        public NotifyMaintenance(IEmailProvider emailProvider, BaseEmailSettings baseSettings, ILogger logger=null)
        {
            _emailProvider = emailProvider;
            _logger = logger;
            _baseSettings = baseSettings;
        }
        public void NotifyExceptionToDebugger(Exception exception, string classFunctionName = "")
        {
            try
            {
                string errorMsg =  $"Exception Details={ExceptionUtils.FormatException(exception, includeContext: true)}";

                string sub = $"Exception position={classFunctionName}";
                _emailProvider.SendMail(_baseSettings.BugRecipientEmailAddress, sub, errorMsg);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex,"NotifyMaintenance met error ({0}) at Framework.ExceptionHandler.NotifyExceptionToDebugger ", ex.ReportAllProperties());
            }

        }

        public void LogAndNotifyToDebugger(Exception exception, string classFunctionName = "")
        {
            _logger?.LogError("Program met error ({0}) at ({1}) ", exception.ReportAllProperties(), classFunctionName);
            NotifyExceptionToDebugger(exception, classFunctionName);
        }

        public void NotifyInfoToDebugger(string classFunctionName = "", string otherInfo = "")
        {
            try
            {
                string sub = $"Exception position={classFunctionName}";
                _emailProvider.SendMail(_baseSettings.BugRecipientEmailAddress, sub, otherInfo);
            }
            catch (Exception ex)
            {
                _logger?.LogError("NotifyMaintenance met error ({0}) at Framework.ExceptionHandler.NotifyInfoToDebugger ", ex.ReportAllProperties());
            }
        }

       
        public ILogger Logger => _logger;
    }
}