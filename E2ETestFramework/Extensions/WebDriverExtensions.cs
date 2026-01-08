using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace E2ETestFramework.Extensions
{
    public static class WebDriverExtensions
    {
        /// <summary>
        /// Sets the specified value..
        /// </summary>
        /// <param name="wait">The referenced web driver wait.</param>
        /// <param name="locator">The web element.</param>
        /// <param name="value">The value to be set.</param>
        public static void SetValue(this WebDriverWait wait, By locator, string value)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            element.SendKeys(value);
        }
        public static void Click(this WebDriverWait wait, By locator)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            element.Click();
        }

        public static bool IsDisplayed(this WebDriverWait wait, By locator)
        {
            var element = wait.Until(ExpectedConditions.ElementIsVisible(locator));
            return element.Displayed;
        }
    }
}
