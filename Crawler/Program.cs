using Crawler.Logics;
using System;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var logics = new CrawlerLogics();
            logics.DownloadAllReports();

            Console.WriteLine("Download done!");
        }
    }
}
