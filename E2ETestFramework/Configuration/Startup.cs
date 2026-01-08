using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using E2ETestFramework.Configuration;
using E2ETestFramework.Infrastructure;
using E2ETestFramework.Services;
using E2ETestFramework.Pages;

namespace E2ETestFramework
{
    public class Startup
    {    
        public void ConfigureServices(IServiceCollection services)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").AddUserSecrets<Startup>().Build();

            var testSettings = new TestSettings();
            config.GetSection("TestSettings").Bind(testSettings);
            config.GetSection("TestUser").Bind(testSettings.TestUser);
            services.AddSingleton(testSettings);

            // WebDriver Registration (Scoped means one instance per test)
            services.AddScoped<IWebDriver>(provider => BrowserFactory.CreateDriver(provider.GetRequiredService<TestSettings>()));

            // Bind MailTmSettings from appsettings.json
            var mailTmSettings = config.GetSection("MailTmSettings").Get<MailTmSettings>()!;
            services.AddSingleton(mailTmSettings);

            // Register HttpClient and our new EmailService
            services.AddHttpClient<EmailService>();

            // Page Object Registration
            services.AddScoped<LoginPage>();
            services.AddScoped<TwoFactorPage>();
            services.AddScoped<DashboardPage>();
        }
    }
}
