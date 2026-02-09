using InterviewTask.Utilities;
using Microsoft.Playwright;

namespace InterviewTask.Models;

public class WaitlistPage
{
    private readonly IPage _page;
    public string? DialogMessage { get; private set; }

    #region Step 1 Form Elements
    private readonly ILocator _firstNameInput;
    private readonly ILocator _lastNameInput;
    private readonly ILocator _emailInput;
    private readonly ILocator _nextButton;
    #endregion
    
    #region Step 2 Form Elements
    private readonly ILocator _professionalButton;
    private readonly ILocator _industryDropdown;
    private readonly ILocator _companyDropdown;
    private readonly ILocator _jobDropdown;
    private readonly ILocator _projectDropdown;
    private readonly ILocator _projectTextbox;
    private readonly ILocator _updatesCheckbox;
    private readonly ILocator _agreementCheckbox;
    private readonly ILocator _joinWaitlistButton;
    #endregion

    public WaitlistPage(IPage page)
    {
        _page = page;

        // Initialize Step 1 elements
        _firstNameInput = page.Locator("#First-Name");
        _lastNameInput = page.Locator("#Last-Name");
        _emailInput = page.Locator("#Email-Address");
        _nextButton = page.Locator("#Next-form");

        // Initialize Step 2 elements
        _professionalButton = page.GetByText("I Am A Professional");
        _industryDropdown = page.GetByLabel("Industry");
        _companyDropdown = page.GetByLabel("Company Size");
        _jobDropdown = page.GetByLabel("Job Function");
        _projectDropdown = page.GetByLabel("Current Or Future Project");
        _projectTextbox = page.Locator("#Describe-your-project");
        _updatesCheckbox = page.Locator("#communications-consent");
        _agreementCheckbox = page.Locator("#Email-Consent");
        _joinWaitlistButton = page.Locator("input[value='JOIN THE WAITLIST']");

        AlertHelper.SetupAlertHandler(_page, message => DialogMessage = message);
    }

    // Navigation
    public async Task NavigateAsync(string url) => await _page.GotoAsync(url);

    // Step 1 - Basic Information
    public async Task FillFirstNameAsync(string text) => await _firstNameInput.FillAsync(text);

    public async Task FillLastNameAsync(string text) => await _lastNameInput.FillAsync(text);

    public async Task FillEmailAsync(string text) => await _emailInput.FillAsync(text);

    public async Task ClickNextAsync() => await _nextButton.ClickAsync();

    // Step 2 - Professional Information
    public async Task ClickProfessionalAsync() => await _professionalButton.ClickAsync();

    public async Task SelectIndustryAsync(string option) => await _industryDropdown.SelectOptionAsync(option);

    public async Task SelectCompanySizeAsync(string option) => await _companyDropdown.SelectOptionAsync(option);

    public async Task SelectJobFunctionAsync(string option) => await _jobDropdown.SelectOptionAsync(option);

    public async Task SelectProjectAsync(string option) => await _projectDropdown.SelectOptionAsync(option);

    public async Task FillProjectDescriptionAsync(string text) => await _projectTextbox.FillAsync(text);

    // Step 3 - Consent and Submission
    public async Task ClickUpdatesCheckboxAsync() => await _updatesCheckbox.ClickAsync();

    public async Task ClickAgreementCheckboxAsync() => await _agreementCheckbox.ClickAsync();

    public async Task ClickJoinWaitlistAsync() => await _joinWaitlistButton.ClickAsync();
}