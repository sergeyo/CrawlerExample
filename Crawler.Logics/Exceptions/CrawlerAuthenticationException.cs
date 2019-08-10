using System;

namespace Crawler.Logics.Exceptions
{
    class CrawlerAuthenticationException : Exception
    {
        public CrawlerAuthenticationException(string message) : base(message)
        {
        }
    }
}
