using System;
using System.Threading;

namespace Crawler.Logics.Authentication
{
    public class AuthenticationService
    {
        private const int retryCount = 30;
        private readonly IMailServiceFactory _mailServiceFactory;
        private readonly IAuthLinkStore _authLinkStore;

        internal AuthenticationService(IMailServiceFactory mailServiceFactory, IAuthLinkStore authLinkStore)
        {
            _mailServiceFactory = mailServiceFactory;
            _authLinkStore = authLinkStore;
        }

        public static AuthenticationService Create()
        {
            return new AuthenticationService(new MailServiceFactory(), new AuthLinkStore());
        }
                
        public string GetLastAuthUrl()
        {
            return _authLinkStore.ReadLastAuthUrl();
        }

        public string GetNextAuthUrl()
        {
            var lastUrl = _authLinkStore.ReadLastAuthUrl();
            using (var mailService = _mailServiceFactory.CreateMailService())
            {
                for (var i = 0; i < retryCount; i++)
                {
                    Thread.Sleep(1000);
                    var linkFromMail = mailService.FindRecentAuthLink();
                    if (linkFromMail != lastUrl)
                    {
                        _authLinkStore.SaveLastAuthUrl(linkFromMail);
                        return linkFromMail;
                    }
                }
                throw new Exception($"We didn't receive the mail message with the authentication URL after { retryCount } attempts.");
            }
        }
    }
}
