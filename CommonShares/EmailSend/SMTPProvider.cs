using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Logging;

namespace CommonShares.EmailSend
{
    /// <summary>Attention, this class can only be used with SMTP correctly setting in configuration files
    /// 
    /// </summary>
    public class SMTPProvider : IEmailProvider
    {
        #region private member

        private readonly string _defaultFromEmail;
        private readonly string _smtpServer;
        private readonly int _smtpPort = 587;
        private readonly string _smtpUserName;
        private readonly string _smtpPassword;

        private readonly Encoding _Encoding = Encoding.Default;

        private readonly ILogger _logger;
        private readonly string _emailDefaultDisplayName;
        private readonly string _defaultEmailSubject;
        private readonly string _replyToTarget;
        private readonly EmailValidationSpecification _emailValidationSpecification;
        #endregion

        public SMTPProvider( BaseEmailSettings basicSettings, EmailValidationSpecification emailValidationSpecification,
            ILogger logger=null)
        {
            _logger = logger;

            _emailDefaultDisplayName = basicSettings.EmailDefaultDisplayName;
            _defaultFromEmail = basicSettings.DefaultFromEmail;
            _defaultEmailSubject = basicSettings.DefaultEmailSubject;
            _replyToTarget = basicSettings.ReplyToTarget;

            _smtpServer = basicSettings.SmtpPrimaryHost;
            _smtpPort = basicSettings.SmtpPrimaryPort;
            _smtpUserName = basicSettings.SmtpPrimaryUsername;
            _smtpPassword = basicSettings.SmtpPrimaryPassword;

            _emailValidationSpecification = emailValidationSpecification;

        }

        public SMTPProvider(string fromEmail, string smtpServer, int smtpPort, string smtpUserName, string smtpPassword, Encoding encode)
        {
            this._defaultFromEmail = fromEmail;
            this._smtpServer = smtpServer;
            this._smtpPort = smtpPort;
            this._smtpUserName = smtpUserName;
            this._smtpPassword = smtpPassword;
            this._Encoding = encode;
            _emailValidationSpecification = new EmailValidationSpecification();
        }
        public SMTPProvider(string fromEmail, string smtpServer, string smtpUserName, string smtpPassword)
        {
            this._defaultFromEmail = fromEmail;
            this._smtpServer = smtpServer;
            this._smtpUserName = smtpUserName;
            this._smtpPassword = smtpPassword;
            _emailValidationSpecification = new EmailValidationSpecification();
        }

        private bool IsValidateAddresses(string to, string ccTo)
        {
            if (string.IsNullOrWhiteSpace(to) || !_emailValidationSpecification.IsSatisfiedBy(to))
            {
                _logger?.LogError("Email address {0} is not a valid address", to);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ccTo) || !_emailValidationSpecification.IsSatisfiedBy(ccTo))
            {
                _logger?.LogError("Email address {0} is not a valid address", ccTo);
                return false;
            }
            return true;
        }
        private bool IsValidateAddresses(string to)
        {
            if (string.IsNullOrWhiteSpace(to) || !_emailValidationSpecification.IsSatisfiedBy(to))
            {
                _logger?.LogError("Email address {0} is not a valid address", to);
                return false;
            }

            return true;
        }

        public bool SendMail(string from, string to, string subject, string body)
        {
            if (!IsValidateAddresses(to))
            {
                return false;
            }

            try
            {
                MailMessage message = GenerateMessage(from, to, subject, body);
                SendMail(message);
            }
            catch (Exception exception)
            {
                _logger?.LogError(exception, $"SendMail(string from, string to, string subject, string body) met Exception = ({exception})");
                return false;
            }
            return true;
        }

        private MailMessage GenerateMessage(string from, string to, string subject, string body, string ccTo = null, List<string> bccTo = null)
        {
            var fromAddress = new MailAddress(from, _emailDefaultDisplayName);
            var toAddress = new MailAddress(to);
            MailMessage message = new MailMessage(fromAddress, toAddress);
            message.Body = body;            

            string finalSub = GetFinalSubject(subject);
            message.Subject = finalSub;

            message.SubjectEncoding = _Encoding;
            message.BodyEncoding = _Encoding;
            message.IsBodyHtml = true;
            //message.Priority = MailPriority.High;

            if (!string.IsNullOrEmpty(_replyToTarget))
            {
                message.ReplyToList.Add(_replyToTarget);
            }

            if (!string.IsNullOrEmpty(ccTo))
            {
                message.CC.Add(ccTo);
            }

            if (bccTo != null)
            {
                foreach (string email in bccTo)
                {
                    message.Bcc.Add(email);
                }
            }

            return message;
        }

        private string GetFinalSubject(string subject)
        {
            string sub = string.IsNullOrEmpty(subject) ? string.Empty : subject;
            int len = sub.Length > 168 ? 168 : sub.Length;
            string finalSub = sub.Substring(0, len).Replace('\r', ' ').Replace('\n', ' ');
            return finalSub;
        }

        private void SendMail(MailMessage message)
        {
            using (SmtpClient smtp = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(_smtpUserName, _smtpPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Send(message);
            }
        }

        public bool SendMail(string from, string to, string ccTo, string subject, string body)
        {
            if (!IsValidateAddresses(to, ccTo))
            {
                return false;
            }

            try
            {
                MailMessage message = GenerateMessage(from, to, subject, body, ccTo);
                SendMail(message);

            }
            catch (Exception exception)
            {
                _logger?.LogError(exception, $"SendMail(string from, string to, string ccTo, string subject, string body) met Exception = ({exception})");
                return false;
            }
            return true;
        }


        public bool SendMail(string to, string subject, string body)
        {
            return SendMail(_defaultFromEmail, to, subject, body);
        }

        public bool SendMail(string to, string body)
        {
            return SendMail(to, _defaultEmailSubject, body);
        }


        public bool SendBlindMail(List<string> emailList, string subject, string body)
        {
            try
            {
                if (emailList == null || emailList.Count == 0)
                {
                    return false;
                }
                string toAddress;
                List<string> bccAddresses;
                AssignToAndBccAddresses(emailList, out toAddress, out bccAddresses);
                if (!string.IsNullOrEmpty(toAddress))
                {
                    MailMessage message = GenerateMessage(_defaultFromEmail, toAddress, subject, body, "", bccAddresses);
                    SendMail(message);
                }

            }
            catch (Exception exception)
            {
                _logger?.LogError(exception,"SendBlindMail met Exception = ({0})", exception);
                return false;
            }
            return true;
        }

        private void AssignToAndBccAddresses(List<string> emailList, out string toAddress, out List<string> bccAddresses)
        {
            toAddress = "";
            bccAddresses = new List<string>();
            if (emailList != null)
            {
                foreach (string email in emailList)
                {
                    if (_emailValidationSpecification.IsSatisfiedBy(email))
                    {
                        if (string.IsNullOrEmpty(toAddress))
                        {
                            toAddress = email;
                        }
                        else
                        {
                            bccAddresses.Add(email);
                        }
                    }
                }
            }
        }
    }
}