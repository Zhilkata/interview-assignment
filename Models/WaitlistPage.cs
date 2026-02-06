using Microsoft.Playwright;

namespace RoseticTask.Models;

public class WaitlistPage
{
    private readonly IPage _page;
    public string? DialogMessage { get; private set; }
    
    private readonly ILocator _firstNameInput;
    private readonly ILocator _lastNameInput;
    private readonly ILocator _emailInput;
    private readonly ILocator _nextButton;

    private readonly ILocator _professionalButton;
    
    private readonly ILocator _industryDropdown;
    private readonly ILocator _companyDropdown;
    private readonly ILocator _jobDropdown;
    private readonly ILocator _projectDropdown;

    private readonly ILocator _projectTextbox;

    private readonly ILocator _updatesChechbox;
    private readonly ILocator _agreementChechbox;

    private readonly ILocator _joinWaitlistButton;

    public WaitlistPage(IPage page)
    {
        _page = page;
        SetupDialogueHandler();
        _firstNameInput = page.Locator("#First-Name");
        _lastNameInput = page.Locator("#Last-Name");
        _emailInput = page.Locator("#Email-Address");
        _nextButton = page.Locator("#Next-form");
        
        _professionalButton = page.GetByText("I Am A Professional");

        _industryDropdown = page.GetByLabel("Industry");
        _companyDropdown = page.GetByLabel("Company Size");
        _jobDropdown = page.GetByLabel("Job Function");
        _projectDropdown = page.GetByLabel("Current Or Future Project");
        
        _projectTextbox = page.Locator("#Describe-your-project");
        
        _updatesChechbox = page.Locator("#communications-consent");
        _agreementChechbox = page.Locator("#Email-Consent");

        _joinWaitlistButton = page.Locator("input[value='JOIN THE WAITLIST']");
    }

    public async Task GotoAsync()
    {
        await _page.GotoAsync("https://www.rosetic.ai/#waitlist-form");
    }

    public async Task FillFirstNameAsync(string text)
    {
        await _firstNameInput.ClickAsync();
        await _firstNameInput.FillAsync(text);
    }

    public async Task FillLastNameAsync(string text)
    {
        await _lastNameInput.ClickAsync();
        await _lastNameInput.FillAsync(text);
    }
    
    public async Task FillEmailAsync(string text)
    {
        await _emailInput.ClickAsync();
        await _emailInput.FillAsync(text);
    }

    public async Task ClickNextButtonAsync()
    {
        await _nextButton.ClickAsync();
    }

    public async Task ClickProfessionalButtonAsync()
    {
        await _professionalButton.ClickAsync();
    }

    public async Task SelectIndustryDropdownOptionAsync(string text)
    {
        await _industryDropdown.SelectOptionAsync(text);
    }

    public async Task SelectCompanyDropdownOptionAsync(string text)
    {
        await _companyDropdown.SelectOptionAsync(text);
    }
    
    public async Task SelectJobDropdownOptionAsync(string text)
    {
        await _jobDropdown.SelectOptionAsync(text);
    }
    public async Task SelectProjectDropdownOptionAsync(string text)
    {
        await _projectDropdown.SelectOptionAsync(text);
    }
    
    public async Task FillProjectDescriptionAsync(string text)
    {
        await _projectTextbox.ClickAsync();
        await _projectTextbox.FillAsync(text);
    }

    public async Task ClickUpdatesCheckBoxAsync()
    {
        await _updatesChechbox.ClickAsync();
    }

    public async Task ClickAgreementCheckBoxAsync()
    {
        await _agreementChechbox.ClickAsync();
    }

    public async Task ClickJoinWaitlistButtonAsync()
    {
        await _joinWaitlistButton.ClickAsync();
    }

    private void SetupDialogueHandler()
    {
        _page.Dialog += async (_, dialog) =>
        {
            DialogMessage = dialog.Message;
            await dialog.DismissAsync();
        };
    }
}