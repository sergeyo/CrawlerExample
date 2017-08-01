using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Crawler.Logics.SitePageObjects
{
    class MainPageObject
    {
        private IWebDriver _webDriver;

        public MainPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public static MainPageObject Create(IWebDriver driver)
        {
            return PageFactory.InitElements<MainPageObject>(driver);
        }

        [FindsBy(How = How.CssSelector, Using = "a[href='/User/Logout']")]
        private IList<IWebElement> logoutLink;

        [FindsBy(How = How.Id, Using = "submitbtn")]
        private IWebElement submitButton;

        [FindsBy(How = How.CssSelector, Using = "button.close[data-dismiss='modal']")]
        private IList<IWebElement> modalCloseButtons;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='link-1'] a")]
        private IWebElement downloadPageLink1;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='link-2'] a")]
        private IWebElement downloadPageLink2;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='link-3'] a")]
        private IWebElement downloadPageLink3;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='link-4'] a")]
        private IWebElement downloadPageLink4;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='link-5'] a")]
        private IWebElement downloadPageLink5;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='more-link-1'] a")]
        private IWebElement moreDownloadPageLink1;

        [FindsBy(How = How.CssSelector, Using = "li[data-active='more-link-2'] a")]
        private IWebElement moreDownloadPageLink2;

        //Actually, these links are the same on any page of the site
        [FindsBy(How = How.CssSelector, Using = "a.bs-navbar-brand")]
        private IWebElement MainPageLink;

        public bool IsAuthenticated
        {
            get
            {
                return logoutLink.Any();
            }
        }

        public void SubmitLoginLinkToMailSending()
        {
            submitButton.Click();
            Thread.Sleep(2000);
        }

        public void CloseModalIfNeeded()
        {
            //Wait for modal appearance animation to complete
            Thread.Sleep(4000);

            var visibleButtons = modalCloseButtons.Where(b => b.Displayed).ToList();

            if (!visibleButtons.Any()) return;

            foreach (var button in visibleButtons)
            {
                button.Click();
            }

            //Wait for modal hiding animation to complete
            Thread.Sleep(4000);
        }

        public void ProceedToDownloadPageLink1()
        {
            downloadPageLink1.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToDownloadPageLink2()
        {
            downloadPageLink2.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToDownloadPageLink3()
        {
            downloadPageLink3.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToDownloadPageLink5()
        {
            downloadPageLink5.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToDownloadPageLink4()
        {
            downloadPageLink4.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToMoreDownloadPageLink1()
        {
            moreDownloadPageLink1.Click();
            Thread.Sleep(2000);
        }

        public void ProceedToMoreDownloadPageLink2()
        {
            moreDownloadPageLink2.Click();
            Thread.Sleep(2000);
        }
        
        public void GoToMainPage()
        {
            MainPageLink.Click();
            Thread.Sleep(2000);
        }
    }
}
