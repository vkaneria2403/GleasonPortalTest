using E2ETestFramework.Pages;
using E2ETestFramework.Utilities;
using OpenQA.Selenium;

namespace E2ETestFramework.Tests
{
    public class LoginTests : IDisposable
    {
        // Private fields to hold all the injected dependencies.
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly TestSettings _testSettings;

        // The constructor where the DI container automatically injects all required services.
        // This is managed by the Xunit.DependencyInjection package and our Startup.cs.
        public LoginTests(IWebDriver driver, LoginPage loginPage, TestSettings testSettings)
        {
            _driver = driver;
            _loginPage = loginPage;
            _testSettings = testSettings;
        }

        [Fact]
        public void StandardUser_CanLogin_Successfully()
        {
            // ARRANGE: Set up the initial state for the test.
            _driver.Navigate().GoToUrl(_testSettings.BaseUrl);

            // ACT: Perform the user action being tested.
            _loginPage.LoginAs("vkaneria", "Gleason12#");
        }

        // The Dispose method is called automatically by xUnit after all tests
        // in this class have finished, ensuring the browser is always closed.
        public void Dispose()
        {
            _driver.Quit();
            _driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}