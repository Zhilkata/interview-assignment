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
        await waitlist.NavigateToAsync("https://www.rosetic.ai/#waitlist-form");
        
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
        
        /*Very rudimentary mock setup that simulates submitting the form and
        hitting an endpoint with the following message. I don't click on the button due to requirement. */
        
        await Page.RouteAsync("**/www.rosetic.ai/**", async route =>
        {
            var json = new[] { new { status = 200, message = "You've joined the waitlist!" } };
            await route.FulfillAsync(new()
            {
                Json = json
            });
        });
        
        var responseTask = Page.WaitForResponseAsync("**/www.rosetic.ai/**");
        
        await Page.GotoAsync("https://www.rosetic.ai/");
        
        var responseBody = await responseTask;
        var bodyJson = await responseBody.JsonAsync();
        var message = bodyJson.Value[0].GetProperty("message").GetString();
        Assert.IsTrue(message.Contains("You've joined the waitlist!"));
    }

    [TestMethod]
    //TODO - utilize DataRow functionality
    public async Task WaitlistValidationTest()
    {
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.NavigateToAsync("https://www.rosetic.ai/");
        
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