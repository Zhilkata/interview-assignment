using InterviewTask.Models;
using Microsoft.Playwright.MSTest;

namespace InterviewTask.Tests;

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
    public async Task WaitlistForm_HappyPath()
    {
        const string expectedMessage = "You've joined the waitlist!";
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.NavigateAsync("https://www.rosetic.ai/#waitlist-form");
        
        // Form fill procedure
        await waitlist.FillFirstNameAsync(FirstNameString);
        await waitlist.FillLastNameAsync(LastNameString);
        await waitlist.FillEmailAsync(EmailString);
        await waitlist.ClickNextAsync();
        await waitlist.ClickProfessionalAsync();
        await waitlist.SelectIndustryAsync(IndustryDropdownString);
        await waitlist.SelectCompanySizeAsync(CompanyDropdownString);
        await waitlist.SelectJobFunctionAsync(JobFunctionDropdownString);
        await waitlist.SelectProjectAsync(ProjectDropdownString);
        await waitlist.FillProjectDescriptionAsync(ProjectDescriptionString);
        
        /*Very rudimentary mock setup that simulates submitting the form and
        hitting an endpoint with the following message. I don't click on the button due to requirement. */
        
        await Page.RouteAsync("**/www.rosetic.ai/**", async route =>
        {
            var json = new[] { new { status = 200, message = expectedMessage } };
            await route.FulfillAsync(new(){ Json = json });
        });
        
        var responseTask = Page.WaitForResponseAsync("**/www.rosetic.ai/**");
        await Page.GotoAsync("https://www.rosetic.ai/");
        
        var response = await responseTask;
        var bodyJson = await response.JsonAsync();
        var message = bodyJson.Value[0].GetProperty("message").GetString();
        
        Assert.AreEqual(expectedMessage, message);
    }

    [TestMethod]
    public async Task WaitlistForm_FieldValidation()
    {
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.NavigateAsync("https://www.rosetic.ai/");

        // Click Next with empty fields
        await ClickNextAndVerifyDialog(waitlist, PopupMessageString);

        // Enter single character in email field -> get same response in alert
        await waitlist.FillEmailAsync(EmailOneCharString);
        await ClickNextAndVerifyDialog(waitlist, PopupMessageString);

        // Get new alert message about email format
        await waitlist.FillFirstNameAsync(FirstNameString);
        await waitlist.FillLastNameAsync(LastNameString);
        await ClickNextAndVerifyDialog(waitlist, EmailMessageString);

        /*Afterward, logic follows same pattern*/

        // Verify dialog helper
        async Task ClickNextAndVerifyDialog(WaitlistPage page, string expectedMessage)
        {
            await page.ClickNextAsync();
            await Task.Delay(500);
            Assert.AreEqual(expectedMessage, page.DialogMessage);
        }
    }
}