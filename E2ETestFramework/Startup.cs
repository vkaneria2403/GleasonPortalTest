using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using E2ETestFramework.Utilities;
using E2ETestFramework.Pages;

namespace E2ETestFramework
{
    public class Startup
    {    
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var testSettings = config.GetSection("TestSettings").Get<TestSettings>()!;
            services.AddSingleton(testSettings);

            // 2. WebDriver Registration (Scoped means one instance per test)
            services.AddScoped<IWebDriver>(provider => BrowserFactory.CreateDriver(provider.GetRequiredService<TestSettings>()));

            // 3. Page Object Registration
            services.AddScoped<LoginPage>();
        }
    }
}
