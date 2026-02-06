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
    [TestCategory("End-to-End")]
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
    
    [DataTestMethod]
    [DataRow("", "", "", PopupMessageString, DisplayName = "Empty fields validation")]
    [DataRow(EmailOneCharString, "", "", PopupMessageString, DisplayName = "Single character email validation")]
    [DataRow(EmailOneCharString, FirstNameString, LastNameString, EmailMessageString, DisplayName = "Invalid email format validation")]
    public async Task WaitlistValidationTest(string email, string firstName, string lastName, string expectedMessage)
    {
        var waitlist = new WaitlistPage(await Browser.NewPageAsync());
        await waitlist.NavigateToAsync("https://www.rosetic.ai/");
    
        if (!string.IsNullOrEmpty(email))
            await waitlist.FillEmailAsync(email);
    
        if (!string.IsNullOrEmpty(firstName))
            await waitlist.FillFirstNameAsync(firstName);
    
        if (!string.IsNullOrEmpty(lastName))
            await waitlist.FillLastNameAsync(lastName);
    
        await waitlist.ClickNextButtonAsync();
    
        Assert.AreEqual(expectedMessage, waitlist.DialogMessage);
        
        /*Now DataRow is utilized, but logic won't be as straightforward as it was initially*/
    }
}