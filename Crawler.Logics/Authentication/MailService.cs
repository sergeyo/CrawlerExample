using System;
using System.Linq;
using System.Configuration;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Text.RegularExpressions;
using Crawler.Logics.Exceptions;

namespace Crawler.Logics.Authentication
{
    public class MailService : IMailService
    {
        private static readonly Regex loginUrlRegex = new Regex(@"https://site.com/User/Login/session=[A-Za-z0-9]+", RegexOptions.Compiled);
        private readonly IMailStore _clientImap;

        internal MailService(IMailStore clientImap)
        {
            _clientImap = clientImap;
        }

        public MailService()
        {
            var mail_host = ConfigurationManager.AppSettings["mail_host"];
            var mail_port = int.Parse(ConfigurationManager.AppSettings["mail_port"]);
            var mail_usessl = bool.Parse(ConfigurationManager.AppSettings["mail_usessl"]);
            var mail_user = ConfigurationManager.AppSettings["mail_user"];
            var mail_password = ConfigurationManager.AppSettings["mail_password"];

            _clientImap = new ImapClient(new ProtocolLogger("maillog.txt"));
            _clientImap.ServerCertificateValidationCallback = (s, c, h, e) => true;
            _clientImap.Connect(mail_host, mail_port, mail_usessl);
            _clientImap.AuthenticationMechanisms.Remove("XOAUTH2");
            _clientImap.Authenticate(mail_user, mail_password);
        }

        public string FindRecentAuthLink() {
            var inbox = _clientImap.Inbox;
            inbox.Open(FolderAccess.ReadOnly);

            var messageUids = inbox.Search(SearchQuery.DeliveredAfter(DateTime.Today).And(SearchQuery.SubjectContains("LoginLink")));
            if (!messageUids.Any())
            {
                messageUids = inbox.Search(SearchQuery.DeliveredAfter(DateTime.Today.AddDays(-32)).And(SearchQuery.SubjectContains("LoginLink")));
            }

            var mostRecentLoginUrlMessage = messageUids.Select(muid => inbox.GetMessage(muid))
                                                       .OrderByDescending(m => m.Date)
                                                       .FirstOrDefault();

            if (mostRecentLoginUrlMessage == null) throw new CrawlerAuthenticationException("No recent LoginLink letters found in Inbox!");

            var match = loginUrlRegex.Match(mostRecentLoginUrlMessage.TextBody);

            if (!match.Success)
            {
                throw new CrawlerAuthenticationException("No login link found in the last LoginLink letter!");
            }

            return match.Value;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            try
            {
                _clientImap.Disconnect(true);
            }
            catch (Exception)
            {
                // justi ignore because if connection is already down we don't need it
            }

            _clientImap.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
