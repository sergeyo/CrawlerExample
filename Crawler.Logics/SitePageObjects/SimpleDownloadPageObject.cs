using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Threading;

namespace Crawler.Logics.SitePageObjects
{
    class SimpleDownloadPageObject
    {
        public SimpleDownloadPageObject(IWebDriver driver)
        {

        }

        public static SimpleDownloadPageObject Create(IWebDriver driver)
        {
            return PageFactory.InitElements<SimpleDownloadPageObject>(driver);
        }

        [FindsBy(How = How.CssSelector, Using = "a.xls-download,a.xlsdownload")]
        private IWebElement excelReportButton;

        public void ClickExcelReportButton()
        {
            excelReportButton.Click();
            Thread.Sleep(2000);
        }
    }
}
