namespace E2ETestFramework.Utilities
{
    public class TestSettings
    {
        public string BaseUrl { get; set; } = string.Empty; // Default value, can be overridden
        public string BrowserType { get; set; } = string.Empty; // Default browser, can be overridden
        public bool IsHeadless { get; set; } // Default environment, can be overridden
        public int TimeoutInSeconds { get; set; } = 30; // Default timeout for tests
        public bool EnableLogging { get; set; } = true; // Flag to enable or disable logging
        // Add more settings as needed
    }
}
