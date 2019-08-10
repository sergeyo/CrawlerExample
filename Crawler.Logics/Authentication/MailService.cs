using System;
using System.Linq;
using System.Configuration;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Text.RegularExpressions;

namespace Crawler.Logics.Authentication
{
    public class MailService : IMailService
    {
        private string mail_host;
        private int mail_port;
        private bool mail_usessl;
        private string mail_user;
        private string mail_password;
        private readonly IMailStore clientImap;

        public MailService()
        {
            mail_host = ConfigurationManager.AppSettings["mail_host"];
            mail_port = int.Parse(ConfigurationManager.AppSettings["mail_port"]);
            mail_usessl = bool.Parse(ConfigurationManager.AppSettings["mail_usessl"]);
            mail_user = ConfigurationManager.AppSettings["mail_user"];
            mail_password = ConfigurationManager.AppSettings["mail_password"];

            clientImap = new ImapClient(new ProtocolLogger("maillog.txt"));
            clientImap.ServerCertificateValidationCallback = (s, c, h, e) => true;
            clientImap.Connect(mail_host, mail_port, mail_usessl);
            clientImap.AuthenticationMechanisms.Remove("XOAUTH2");
            clientImap.Authenticate(mail_user, mail_password);
        }

        Regex loginUrlRegex = new Regex(@"https://site.com/User/Login/?");
        
        public string FindRecentAuthLink() {
            var inbox = clientImap.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var messageUids = inbox.Search(SearchQuery.DeliveredAfter(DateTime.Today).And(SearchQuery.SubjectContains("LoginLink")));
            if (!messageUids.Any())
            {
                messageUids = inbox.Search(SearchQuery.DeliveredAfter(DateTime.Today.AddDays(-32)).And(SearchQuery.SubjectContains("LoginLink")));
            }

            var mostRecentLoginUrlMessage = messageUids.Select(muid => inbox.GetMessage(muid))
                                                       .OrderByDescending(m => m.Date)
                                                       .FirstOrDefault();

            if (mostRecentLoginUrlMessage == null) throw new Exception("No recent LoginLink letters found in Inbox!");

            var match = loginUrlRegex.Match(mostRecentLoginUrlMessage.TextBody);

            if (!match.Success)
            {
                throw new Exception("No login link found in the last LoginLink letter!");
            }

            return match.Value;
        }

        public void Dispose()
        {
            try {
                clientImap.Disconnect(true);
            }
            catch (Exception) {
                // justi ignore because if connection is already down we don't need it
            }

            clientImap.Dispose();
        }
    }
}
