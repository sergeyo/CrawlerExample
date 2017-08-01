using Crawler.Logics.Authentication;

namespace Crawler.Tests.AutenticationLogics
{
    class AuthLinkStoreStub : IAuthLinkStore
    {
        private string value;

        public AuthLinkStoreStub(string initValue)
        {
            value = initValue;
        }

        public string ReadLastAuthUrl()
        {
            return value;
        }

        public void SaveLastAuthUrl(string url)
        {
            value = url;
        }
    }
}
