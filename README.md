# Gleason Portal E2E Test Framework

This project contains the Automated End-to-End (E2E) test suite for the Gleason GEMS Cloud Portal. It focuses on validating critical user workflows, including 2FA login scenarios via Email OTP.

## 🛠 Tech Stack & Architecture

* **Language:** C# (.NET 8.0)
* **Test Framework:** xUnit (v2)
* **Browser Automation:** Selenium WebDriver
* **Assertions:** FluentAssertions
* **Architecture:**
    * **Page Object Model (POM):** Separation of page elements/actions from test logic.
    * **Dependency Injection (DI):** Manages object lifecycles (`IWebDriver`, Pages, Settings) via `Microsoft.Extensions.DependencyInjection`.
    * **Configuration:** Hybrid approach using `appsettings.json` (templates) and **User Secrets** (sensitive data).
* **Email Testing:** Uses `mail.tm` API to programmatically fetch OTP codes forwarded from the corporate email.

---

## 🚀 Prerequisites

Before running the tests, ensure you have:
1.  **Visual Studio 2022** (or VS Code/Rider).
2.  **.NET 8.0 SDK** installed.
3.  **Chrome Browser** installed.
4.  **cURL** installed (usually pre-installed on Windows/Mac) to generate email credentials.

---

## ⚙️ Setup Guide (Run these steps first!)

Since this project uses **User Secrets** to protect credentials, the project will **NOT** run immediately after cloning. You must configure your local secrets.

### Step 1: Clone and Restore
```bash
git clone https://github.com/vkaneria2403/GleasonPortalTest.git
cd GleasonPortalTest/E2ETestFramework
dotnet restore
```

### Step 2: Generate Mail.tm Credentials
You need a dedicated testing email account on mail.tm. Since mail.tm does not have a UI for registration, run the following commands in your terminal (Command Prompt or PowerShell):

**A. Create the Account** - Replace `your-test-user` and `your-password` with your desired values.
```powershell
curl -X POST "https://api.mail.tm/accounts" `
     -H "Content-Type: application/json" `
     -d "{\"address\":\"your-test-user@powerscrews.com\",\"password\":\"your-password\"}"
```

> **Note:** The domain must be `@powerscrews.com` (or whatever the API currently supports).

**B. Get the API Token** - Use the credentials you just created to get the Token.
```powershell
curl -X POST "https://api.mail.tm/token" `
     -H "Content-Type: application/json" `
     -d "{\"address\":\"your-test-user@powerscrews.com\",\"password\":\"your-password\"}"
```

Copy the `token` string from the response. You will need this for the next step.

### Step 3: Configure User Secrets
We do not store passwords or API tokens in appsettings.json. You must set them locally.

Right-click the project in Visual Studio -> Manage User Secrets.

Paste the following JSON structure:
{
  "TestSettings": {
    "TestUser": {
      "Username": "your.work.email@gleason.com",
      "Password": "YOUR_REAL_WORK_PASSWORD"
    }
  },
  "MailTmSettings": {
    "ApiToken": "PASTE_YOUR_GENERATED_TOKEN_HERE"
  }
}

Step 4: Configure Email Forwarding
Standard Outlook rules are currently blocked by company policy. To receive OTPs in the test framework, you must enable Global Forwarding.

Go to Outlook Web.

Navigate to Settings (⚙️) > Mail > Forwarding.

Toggle Enable forwarding to ON.

Enter your mail.tm address (e.g., your-test-user@powerscrews.com).

IMPORTANT: Check the box "Keep a copy of forwarded messages". (If you miss this, emails will vanish from your work inbox).

Click Save.

> **Important:** This setting forwards ALL incoming email to the test account. Please disable this setting when you are not actively running the test suite.

---

## ### Option 1: Visual Studio Test Explorer
1. Open **Test** > **Test Explorer**
2. Build the solution
3. Click the **Run** (green play) button

### Option 2: Command Line
Open a terminal in the project folder and run:
```bash
dotnet test
```

### Verify User Secrets Configuration
To verify your User Secrets are configured correctly, run:
```bash
cd E2ETestFramework
dotnet user-secrets list
```

You should see:
- `TestUser:Username`
- `TestUser:Password`
- `MailTmSettings:ApiToken`

---

## 🔍 Troubleshooting

### Tests Fail with Empty Credentials
**Problem:** Tests fail because User Secrets are not configured.

**Solution:** 
1. Verify User Secrets are set: `dotnet user-secrets list`
2. Ensure you're running from the correct directory (`E2ETestFramework`)
3. Check that the User Secrets ID matches in `.csproj` file

### OTP Not Found
**Problem:** `GetLatestOtpAsync` times out.

**Solution:**
1. Verify email forwarding is enabled in Outlook
2. Check that forwarding address matches your mail.tm account
3. Ensure "Keep a copy" is checked in forwarding settings
4. Verify API token is valid: Test with cURL or check token expiration

### Browser Driver Issues
**Problem:** WebDriver cannot start browser.

**Solution:**
1. Ensure Chrome/Firefox/Edge is installed
2. Check browser version matches WebDriver version
3. Verify browser is not already running (close all instances)
4. Check for antivirus/firewall blocking WebDriver

---

📂 Project Structure
E2ETestFramework/
├── Configuration/          # Configuration & Settings
│   ├── TestSettings.cs     # Main test configuration class
│   ├── MailTmSettings.cs   # Mail.tm API configuration class
│   ├── TestUserSettings.cs # Test user credentials configuration class
│   └── Startup.cs          # DI Container Configuration
│
├── Services/               # External Service Integrations
│   └── EmailService.cs     # Handles mail.tm API calls to fetch OTPs
│
├── Infrastructure/          # Infrastructure & Factories
│   └── BrowserFactory.cs   # Creates WebDriver instances (Chrome/Edge/Firefox)
│
├── Extensions/             # Extension Methods
│   └── WebDriverExtensions.cs # Custom Extension methods (Waits, clicks)
│
├── Pages/                  # Page Objects (UI Mappings & Actions)
│   ├── BasePage.cs         # Common logic for all pages
│   ├── LoginPage.cs        # Login screen actions
│   ├── TwoFactorPage.cs    # OTP entry actions
│   └── DashboardPage.cs   # Dashboard/Landing page actions
│
├── Tests/                  # Test Classes
│   └── LoginTests.cs       # 2FA Login workflow tests
│
├── TestData/               # Test Data Files (currently empty)
│
├── appsettings.json        # Configuration template (Non-sensitive data)
└── E2ETestFramework.csproj