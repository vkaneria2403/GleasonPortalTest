using E2ETestFramework.Extensions;
using OpenQA.Selenium;

namespace E2ETestFramework.Pages
{
    public class DashboardPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _header = By.CssSelector("div[data-testid='dashboard']");
        public bool IsHeaderVisible()
        {
            try
            {
                return Wait.IsDisplayed(_header);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}
