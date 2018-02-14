using System.Text.RegularExpressions;

namespace CommonShares.EmailSend
{
    public class EmailValidationSpecification
    {
        private static Regex _emailregex = new Regex(EmailRegex);
        public const string EmailRegex = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        public bool IsSatisfiedBy(string email)
        {
            return _emailregex.IsMatch(email);
        }
    }
}
