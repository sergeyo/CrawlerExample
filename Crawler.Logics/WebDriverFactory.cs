using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Configuration;
using System.IO;

namespace Crawler.Logics
{
    internal static class WebDriverFactory
    {
        public static IWebDriver Create()
        {
            var service = ChromeDriverService.CreateDefaultService();
            var chromeOptions = new ChromeOptions();
            if (ConfigurationManager.AppSettings["useProxy"] == "true")
            {
                chromeOptions.Proxy = new Proxy();
                chromeOptions.Proxy.HttpProxy = ConfigurationManager.AppSettings["proxy"];
                chromeOptions.Proxy.SslProxy = ConfigurationManager.AppSettings["proxy"];
                chromeOptions.Proxy.Kind = ProxyKind.Manual;
            }

            var downloadDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Download");
            chromeOptions.AddUserProfilePreference("download.default_directory", downloadDirectory);

            var driver = new ChromeDriver(service, chromeOptions, TimeSpan.FromSeconds(30));
            
            return driver;
        }
    }
}
