using System;

namespace Crawler.Logics.Exceptions
{
    public class CrawlerAuthenticationException : Exception
    {
        public CrawlerAuthenticationException(string message) : base(message)
        {
        }
    }
}
