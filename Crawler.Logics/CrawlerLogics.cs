using Crawler.Logics.Authentication;
using Crawler.Logics.SitePageObjects;
using OpenQA.Selenium;
using System;
using System.Configuration;
using System.Threading;

namespace Crawler.Logics
{
    public class CrawlerLogics
    {
        private static readonly string mainPageUrl;
        static CrawlerLogics()
        {
            mainPageUrl = ConfigurationManager.AppSettings["mainPageLink"];
        }

        public void DownloadAllReports()
        {
            using (IWebDriver driver = WebDriverFactory.Create())
            {
                var authService = AuthenticationService.Create();

                driver.Url = authService.GetLastAuthUrl();
                // wait for user to type login credentials in native modal window
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(60);
                driver.FindElement(By.CssSelector("div.row"));
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var mainPage = MainPageObject.Create(driver);

                PerformAuthentificationIfNeeded(driver, authService, mainPage);

                mainPage.CloseModalIfNeeded();
                mainPage.ProceedToDownloadPageLink1();
                DownloadReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToDownloadPageLink2();
                DownloadReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToDownloadPageLink3();
                DownloadReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToDownloadPageLink4();
                DownloadReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToDownloadPageLink5();
                DownloadSimpleReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToMoreDownloadPageLink1();
                DownloadSimpleReport(driver);

                NavigateToMainPage(driver, mainPage);
                mainPage.ProceedToMoreDownloadPageLink2();
                DownloadSimpleReport(driver);

                //wait for all reports to download
                Thread.Sleep(10000);
            }
        }

        private static void DownloadSimpleReport(IWebDriver driver)
        {
            var simpleDownloadPage = SimpleDownloadPageObject.Create(driver);
            simpleDownloadPage.ClickExcelReportButton();
            Thread.Sleep(5000);
        }

        private static void NavigateToMainPage(IWebDriver driver, MainPageObject mainPage)
        {
            mainPage.GoToMainPage();
            mainPage.CloseModalIfNeeded();
        }

        private static void DownloadReport(IWebDriver driver)
        {
            var downloadPage = DownloadablePageObject.Create(driver);

            downloadPage.DownloadLinkClick();
            downloadPage.SelectAllClickInDownloadModal();
            downloadPage.DownloadClickInDownloadModal();
            downloadPage.WaitForPreparingForDownloadModalToClose();
            Thread.Sleep(5000);
        }

        private static void PerformAuthentificationIfNeeded(IWebDriver driver, AuthenticationService authService, MainPageObject mainPage)
        {
            if (!mainPage.IsAuthenticated)
            {
                mainPage.SubmitLoginLinkToMailSending();
                Thread.Sleep(5000);

                driver.Url = authService.GetNextAuthUrl();

                if (!mainPage.IsAuthenticated)
                {
                    throw new Exception("We have some problems with authentication!");
                }
            }
        }
    }
}
