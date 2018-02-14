using System.Collections.Generic;

namespace CommonShares.EmailSend
{
    public interface IEmailProvider
    {
        bool SendMail(string from, string to, string subject, string body);
        bool SendMail(string from, string to, string ccTo, string subject, string body);
        bool SendMail(string to, string subject, string body);
        bool SendBlindMail(List<string> emailList, string subject, string body);
    }
}
