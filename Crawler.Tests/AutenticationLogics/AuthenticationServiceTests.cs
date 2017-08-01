using Crawler.Logics.Authentication;
using NUnit.Framework;
using Rhino.Mocks;

namespace Crawler.Tests.AutenticationLogics
{
    [TestFixture]
    public class AuthenticationServiceTests
    {
        private const string oldAuthLink = "oldAuthLink";
        private const string newAuthLink = "newAuthLink";

        private IMailServiceFactory CreateMailServiceFactoryMock(string firstAuthString, string secondAuthString)
        {
            var mailService = MockRepository.GenerateMock<IMailService>();

            mailService.Stub(m => m.FindRecentAuthLink()).Return(firstAuthString).Repeat.Twice();
            mailService.Stub(m => m.FindRecentAuthLink()).Return(secondAuthString).Repeat.Once();

            var factory = MockRepository.GenerateStub<IMailServiceFactory>();
            factory.Stub(f => f.CreateMailService()).Return(mailService);

            return factory;
        }

        [Test]
        public void GetNextAuthUrl_ShouldCheckMailService_UntilItReturnsAnotherLink()
        {
            var mailServiceFactory = CreateMailServiceFactoryMock(oldAuthLink, newAuthLink);
            var authService = new AuthenticationService(mailServiceFactory, new AuthLinkStoreStub(oldAuthLink));

            var authLink = authService.GetNextAuthUrl();

            Assert.That(authLink, Is.EqualTo(newAuthLink));
            var mailService = mailServiceFactory.CreateMailService();
            mailService.VerifyAllExpectations();
        }

        [Test]
        public void GetLastAuthUrl_ShouldReturnLastUrlFromStore()
        {
            var mailServiceFactory = CreateMailServiceFactoryMock(oldAuthLink, newAuthLink);
            var authService = new AuthenticationService(mailServiceFactory, new AuthLinkStoreStub(oldAuthLink));

            var authLink = authService.GetLastAuthUrl();

            Assert.That(authLink, Is.EqualTo(oldAuthLink));

            var mailService = mailServiceFactory.CreateMailService();
            mailService.AssertWasNotCalled(m => m.FindRecentAuthLink());
        }
    }
}
