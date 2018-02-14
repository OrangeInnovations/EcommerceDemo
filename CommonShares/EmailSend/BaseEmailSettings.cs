using System;

namespace CommonShares.EmailSend
{
    public class BaseEmailSettings
    {
        public string DefaultFromEmail { get; set; }
        public string EmailDefaultDisplayName { get; set; }
        public string DefaultEmailSubject { get; set; }
        public string ReplyToTarget { get; set; }
        public string SmtpPrimaryHost { get; set; }
        public int SmtpPrimaryPort { get; set; }
        public string SmtpPrimaryUsername { get; set; }
        public string SmtpPrimaryPassword { get; set; }

        public string BugRecipientEmailAddress { get; set; }


        public static BaseEmailSettings GetCloudConnectionSettings()
        {
            throw new NotImplementedException();
        }
    }
}