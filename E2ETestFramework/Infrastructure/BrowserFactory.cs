using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using E2ETestFramework.Configuration;

namespace E2ETestFramework.Infrastructure
{
    public static class BrowserFactory
    {
        public static IWebDriver CreateDriver(TestSettings testSettings)
        {
            switch (testSettings.BrowserType.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();
                    if (testSettings.IsHeadless)
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    return new ChromeDriver(chromeOptions);
                case "firefox":
                    var firefoxOptions = new FirefoxOptions();
                    if (testSettings.IsHeadless)
                    {
                        firefoxOptions.AddArgument("--headless");
                    }
                    return new FirefoxDriver(firefoxOptions);
                case "edge":
                    var edgeOptions = new EdgeOptions();
                    if (testSettings.IsHeadless)
                    {
                        edgeOptions.AddArgument("--headless");
                    }
                    return new EdgeDriver(edgeOptions);
                default:
                    throw new NotSupportedException($"Browser '{testSettings.BrowserType}' is not supported.");
            }
        }
    }
}
