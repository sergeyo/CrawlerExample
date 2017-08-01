using System.Configuration;
using System.IO;

namespace Crawler.Logics.Authentication
{
    class AuthLinkStore : IAuthLinkStore
    {
        internal const string _lastAuthStringFilename = "lastAuthUrl.bin";

        public string ReadLastAuthUrl()
        {
            try
            {
                return File.ReadAllText(_lastAuthStringFilename);
            }
            catch (IOException)
            {
                return ConfigurationManager.AppSettings["loginLink"];
            }
        }

        public void SaveLastAuthUrl(string url)
        {
            File.WriteAllText(_lastAuthStringFilename, url);
        }
    }
}
