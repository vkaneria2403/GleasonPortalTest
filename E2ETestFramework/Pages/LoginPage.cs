using E2ETestFramework.Utilities;
using OpenQA.Selenium;

namespace E2ETestFramework.Pages
{
    public class LoginPage : BasePage
    {
        // The constructor accepts the IWebDriver from the DI container and passes it to the BasePage.
        public LoginPage(IWebDriver driver) : base(driver) { }

        private readonly By _usernameField = By.CssSelector("input[id='username']");
        private readonly By _passwordField = By.CssSelector("input[id='password']");
        private readonly By _loginButton = By.CssSelector("button[type='submit']");

        public void LoginAs(string username, string password)
        {
            Wait.SetValue(_usernameField, username);
            Wait.SetValue(_passwordField, password);
            Wait.Click(_loginButton);
        }
    }
}
