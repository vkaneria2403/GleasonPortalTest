namespace E2ETestFramework.Configuration
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = string.Empty; // Default value, can be overridden
        public string BrowserType { get; set; } = string.Empty; // Default browser, can be overridden
        public bool IsHeadless { get; set; } // Default environment, can be overridden
        public int TimeoutInSeconds { get; set; } = 30; // Default timeout for tests
        public bool EnableLogging { get; set; } = true; // Flag to enable or disable logging
        public string LogLevel { get; set; } = string.Empty; // Default value, can be overridden
        public int RetryCount { get; set; } // Default value, can be overridden
        public TestUserSettings TestUser { get; set; } = new();
        public MailTmSettings MailTmSettings { get; set; } = new();
        // Add more settings as needed
    }
}
