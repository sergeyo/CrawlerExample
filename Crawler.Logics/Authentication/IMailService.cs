using System;

namespace Crawler.Logics.Authentication
{
    internal interface IMailService : IDisposable
    {
        string FindRecentAuthLink();
    }
}