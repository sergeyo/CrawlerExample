using MimeKit;
using MailKit;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Crawler.Logics.Authentication;
using Crawler.Logics.Exceptions;

namespace Crawler.Tests.AutenticationLogics
{
    [TestFixture]
    public class MailServiceTests
    {
        private IMailStore CreateMailStoreMock(string subject, string text)
        {
            var mailFolderMock = MockRepository.GenerateMock<IMailFolder>();
            if (subject != null)
            {
                var part = new TextPart("plain") { Text = text };
                MimeMessage message = new MimeMessage(Enumerable.Empty<InternetAddress>(), Enumerable.Empty<InternetAddress>(), subject, part);
                message.Date = DateTime.Now.AddHours(-1);
                var id = new UniqueId(1);
                mailFolderMock.Stub(s => s.Search(null)).IgnoreArguments().Return(new List<UniqueId>() { id });
                mailFolderMock.Stub(s => s.GetMessage(id)).Return(message);
            } else
            {
                mailFolderMock.Stub(s => s.Search(null)).IgnoreArguments().Return(new List<UniqueId>() { });
            }

            var mailStoreMock = MockRepository.GenerateMock<IMailStore>();
            mailStoreMock.Stub(s => s.Inbox).Return(mailFolderMock);

            return mailStoreMock;
        }

        [Test]
        public void FindRecentAuthLink_WhenNoMessagesFound_ShouldThrowException()
        {
            var mailStore = CreateMailStoreMock(null, null);
            var mailService = new Logics.Authentication.MailService(mailStore);

            TestDelegate action = () => { mailService.FindRecentAuthLink(); };

            Assert.Throws<CrawlerAuthenticationException>(action);
        }

        [Test]
        public void FindRecentAuthLink_WhenMessageWithSubjectFound_ButContainsNoLink_ShouldThrowException()
        {
            var mailStore = CreateMailStoreMock("LoginLink", "error");
            var mailService = new Logics.Authentication.MailService(mailStore);

            TestDelegate action = () => { mailService.FindRecentAuthLink(); };

            Assert.Throws<CrawlerAuthenticationException>(action);
        }

        [Test]
        public void FindRecentAuthLink_WhenCorrectMessageWithSubjectFound_ShouldReturnLinkInText()
        {
            const string link = "https://site.com/User/Login/session=dfskjg4857hwvo47yvtwp38ntv8o437nvw";
            var mailStore = CreateMailStoreMock("LoginLink", "text text " + link + " text text text");
            var mailService = new Logics.Authentication.MailService(mailStore);

            var result = mailService.FindRecentAuthLink();

            Assert.That(result, Is.EqualTo(link));
        }
    }
}
