using System.Text.Json;
using System.Text.RegularExpressions;
using System.Net.Http.Headers;
using E2ETestFramework.Configuration;

namespace E2ETestFramework.Services;

public class EmailService
{
    private readonly HttpClient _httpClient;

    public EmailService(HttpClient httpClient, MailTmSettings settings)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", settings.ApiToken);
    }

    public async Task ClearInboxAsync()
    {
        var listResponse = await _httpClient.GetAsync("https://api.mail.tm/messages");
        listResponse.EnsureSuccessStatusCode();

        var listJsonResponse = await listResponse.Content.ReadAsStringAsync();
        var doc = JsonDocument.Parse(listJsonResponse);
        var messages = doc.RootElement.GetProperty("hydra:member").EnumerateArray().ToList();

        foreach (var message in messages)
        {
            var messageId = message.GetProperty("id").GetString();
            await _httpClient.DeleteAsync($"https://api.mail.tm/messages/{messageId}");
        }
    }

    public async Task<string> GetLatestOtpAsync(int timeoutSeconds = 60)
    {
        // This method remains the same
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        while (stopwatch.Elapsed.TotalSeconds < timeoutSeconds)
        {
            var response = await _httpClient.GetAsync("https://api.mail.tm/messages");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(jsonResponse);
            var messages = doc.RootElement.GetProperty("hydra:member").EnumerateArray().ToList();

            if (messages.Any())
            {
                var messageId = messages.First().GetProperty("id").GetString();
                var messageResponse = await _httpClient.GetAsync($"https://api.mail.tm/messages/{messageId}");
                messageResponse.EnsureSuccessStatusCode();

                var messageJsonResponse = await messageResponse.Content.ReadAsStringAsync();
                var messageDoc = JsonDocument.Parse(messageJsonResponse);
                var emailBody = messageDoc.RootElement.GetProperty("text").GetString();

                var match = Regex.Match(emailBody ?? "", @"(?<=Authentication Code:\s*)\b[a-zA-Z0-9]{6}\b");
                if (match.Success)
                {
                    return match.Value;
                }
            }
            await Task.Delay(5000);
        }

        throw new TimeoutException($"OTP not found in mail.tm inbox after {timeoutSeconds} seconds.");
    }
}