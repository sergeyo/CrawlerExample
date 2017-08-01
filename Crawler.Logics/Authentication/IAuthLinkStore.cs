namespace Crawler.Logics.Authentication
{
    internal interface IAuthLinkStore
    {
        string ReadLastAuthUrl();

        void SaveLastAuthUrl(string url);
    }
}
