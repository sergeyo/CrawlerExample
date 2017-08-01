namespace Crawler.Logics.Authentication
{
    internal interface IMailServiceFactory
    {
        IMailService CreateMailService();
    }
}
