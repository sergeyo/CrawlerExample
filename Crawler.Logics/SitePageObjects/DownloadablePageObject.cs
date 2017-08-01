using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Crawler.Logics.SitePageObjects
{
    class DownloadablePageObject
    {
        public DownloadablePageObject(IWebDriver driver)
        {

        }

        public static DownloadablePageObject Create(IWebDriver driver)
        {
            return PageFactory.InitElements<DownloadablePageObject>(driver);
        }

        [FindsBy(How = How.CssSelector, Using = "a[data-target='#export']")]
        private IWebElement downloadReportButtonLink;

        [FindsBy(How = How.CssSelector, Using = "input[data-selector='#exporttab0']")]
        private IWebElement allSelectorCheckBox;

        [FindsBy(How = How.CssSelector, Using = "input#downloadButton")]
        private IWebElement downloadButton;

        [FindsBy(How = How.CssSelector, Using = "form#exportf")]
        private IList<IWebElement> preparingForDownloadModal;

        public void DownloadLinkClick()
        {
            downloadReportButtonLink.Click();
            Thread.Sleep(2000);
        }

        public void SelectAllClickInDownloadModal()
        {
            allSelectorCheckBox.Click();
            Thread.Sleep(2000);
        }

        public void DownloadClickInDownloadModal()
        {
            downloadButton.Click();
            Thread.Sleep(2000);
        }

        public void WaitForPreparingForDownloadModalToClose()
        {
            Thread.Sleep(2000);
            for (int i = 0; i < 30; i++)
            {
                var visibleElements = preparingForDownloadModal.Where(e => e.Displayed).ToArray();
                if (!visibleElements.Any())
                {
                    return;
                }
                Thread.Sleep(1000);
            }
            Thread.Sleep(2000);
        }
    }
}
