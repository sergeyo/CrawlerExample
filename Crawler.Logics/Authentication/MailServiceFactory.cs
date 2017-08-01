namespace Crawler.Logics.Authentication
{
    class MailServiceFactory : IMailServiceFactory
    {
        public IMailService CreateMailService()
        {
            return new MailService();
        }
    }
}
