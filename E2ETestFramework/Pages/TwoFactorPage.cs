using E2ETestFramework.Extensions;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace E2ETestFramework.Pages
{
    public class TwoFactorPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _otpField = By.CssSelector("input[id='authentication-code']");
        private readonly By _verifyButton = By.CssSelector("button[type='submit']");
        private readonly By _loadingIcon = By.CssSelector("svg[class*='MuiCircularProgress-svg']");
        private readonly By _authenticatorAppNotRegisteredCloseButton = By.XPath("//div[@class='MuiDialogContent-root css-1ty026z']//button[@type='button']");

        public void EnterOtp(string otp)
        {
            Wait.SetValue(_otpField, otp);
            Wait.Click(_verifyButton);
        }

        public void WaitForLoadingToDisappear()
        {
            Wait.Until(ExpectedConditions.InvisibilityOfElementLocated(_loadingIcon));
        }

        public void CloseAuthenticatorAppNotRegisteredPopup()
        {
            Wait.Click(_authenticatorAppNotRegisteredCloseButton);
        }
    }
}
