using E2ETestFramework.Pages;
using E2ETestFramework.Configuration;
using E2ETestFramework.Services;
using FluentAssertions;
using OpenQA.Selenium;

namespace E2ETestFramework.Tests
{
    public class LoginTests(IWebDriver driver, LoginPage loginPage, TestSettings testSettings, TwoFactorPage twoFactorPage, DashboardPage dashboardPage, EmailService emailService) : IDisposable
    {
        [Fact]
        public async Task StandardUser_CanLogin_WithEmailOtp_Successfully()
        {
            // ARRANGE: Set up the initial state for the test.
            await emailService.ClearInboxAsync();
            driver.Navigate().GoToUrl(testSettings.BaseUrl);
            var user = testSettings.TestUser;

            // ACT: Perform the user action being tested.
            loginPage.LoginAs(user.Username, user.Password);
            var otpCode = await emailService.GetLatestOtpAsync();
            twoFactorPage.EnterOtp(otpCode);
            twoFactorPage.WaitForLoadingToDisappear();
            twoFactorPage.CloseAuthenticatorAppNotRegisteredPopup();

            // ASSERT: Verify that the final page after a successful login is visible
            dashboardPage.IsHeaderVisible().Should().BeTrue("the inventory page should be visible after a successful 2FA login.");
        }

        // The Dispose method is called automatically by xUnit after all tests
        // in this class have finished, ensuring the browser is always closed.
        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}