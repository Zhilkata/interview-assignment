using System.Runtime.InteropServices.ComTypes;
using Microsoft.Playwright;
using Microsoft.Playwright.MSTest;
using RoseticTask.Models;

namespace RoseticTask.Tests;

[TestClass]
public class WaitlistTest : PageTest
{
    private const string FirstNameString = "TestFN"; 
    private const string LastNameString = "TestLN";
    private const string EmailString = "test@test.com";
    private const string EmailOneCharString = "t";
    
    private const string PopupMessageString = "Please fill out all fields before proceeding.";
    private const string EmailMessageString = "Please enter a valid email address.";

    private const string IndustryDropdownString = "SaaS / Software";
    private const string CompanyDropdownString = "1 - 10";
    private const string JobFunctionDropdownString = "Engineering / Dev";
    private const string ProjectDropdownString = "Developer Tools (IDEs, version control systems,etc.)";

    private const string ProjectDescriptionString = "Test Text 123@!";
        
    [TestMethod]
    public async Task WaitlistE2ETest()
    {
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.GotoAsync();
        
        await waitlist.FillFirstNameAsync(FirstNameString);
        await waitlist.FillLastNameAsync(LastNameString);
        await waitlist.FillEmailAsync(EmailString);
        await waitlist.ClickNextButtonAsync();

        await waitlist.ClickProfessionalButtonAsync();
        
        await waitlist.SelectIndustryDropdownOptionAsync(IndustryDropdownString);
        await waitlist.SelectCompanyDropdownOptionAsync(CompanyDropdownString);
        await waitlist.SelectJobDropdownOptionAsync(JobFunctionDropdownString);
        await waitlist.SelectProjectDropdownOptionAsync(ProjectDropdownString);
        
        await waitlist.FillProjectDescriptionAsync(ProjectDescriptionString);

        //await waitlist.ClickJoinWaitlistButtonAsync();
    }

    [TestMethod]
    //TODO - utilize DataRow functionality
    public async Task WaitlistValidationTest()
    {
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.GotoAsync();
        
        await waitlist.ClickNextButtonAsync();
        await Task.Delay(500);
        Assert.AreEqual(PopupMessageString, waitlist.DialogMessage);
        
        // Enter single character in email field -> get same response in alert
        await waitlist.FillEmailAsync(EmailOneCharString);
        await waitlist.ClickNextButtonAsync();
        await Task.Delay(500);
        Assert.AreEqual(PopupMessageString, waitlist.DialogMessage);
        
        // Get new alert message about email format
        await waitlist.FillFirstNameAsync(FirstNameString);
        await waitlist.FillLastNameAsync(LastNameString);
        await waitlist.ClickNextButtonAsync();
        await Task.Delay(500);
        Assert.AreEqual(EmailMessageString, waitlist.DialogMessage);
        
        /*Afterward, logic follows same pattern*/
    }
}