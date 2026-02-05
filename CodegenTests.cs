using Microsoft.Playwright.MSTest;
using Microsoft.Playwright;
namespace RoseticTask;

[TestClass]
public class Tests : PageTest
{
    [TestMethod]
    public async Task WaitlistE2E()
    {
        await Page.GotoAsync("https://www.rosetic.ai/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Join waitlist ", Exact = true }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name" }).FillAsync("TestFN");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name" }).FillAsync("TestLN");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("test@test.com");
        await Page.GetByText("Next").ClickAsync();
        await Page.Locator(".w-layout-hflex.waitlist-form_switch-btn").First.ClickAsync();
        await Page.GetByLabel("Industry").SelectOptionAsync(new[] { "SaaS / Software" });
        await Page.GetByLabel("Company Size").SelectOptionAsync(new[] { "1 - 10" });
        await Page.GetByLabel("Job Function").SelectOptionAsync(new[] { "Engineering / Dev" });
        await Page.GetByLabel("Current or Future Project").SelectOptionAsync(new[] { "Enterprise Applications (ERP systems, CRM platforms,etc.) " });
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Describe Your Project In a" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Describe Your Project In a" }).FillAsync("test123");
        //await Page.GetByRole(AriaRole.Button, new() { Name = "JOIN THE WISHLIST" }).ClickAsync();
    }

    [TestMethod]
    public async Task WaitlistValidation()
    {
        await Page.GotoAsync("https://www.rosetic.ai/");
        await Page.GetByRole(AriaRole.Link, new() { Name = "Join waitlist ", Exact = true }).ClickAsync();
        void Page_Dialog_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog_EventHandler;
        }
        Page.Dialog += Page_Dialog_EventHandler;
        await Page.GetByText("Next").ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("t");
        void Page_Dialog1_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog1_EventHandler;
        }
        Page.Dialog += Page_Dialog1_EventHandler;
        await Page.GetByText("Next").ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "First Name" }).FillAsync("t");
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Last Name" }).FillAsync("t");
        void Page_Dialog2_EventHandler(object sender, IDialog dialog)
        {
            Console.WriteLine($"Dialog message: {dialog.Message}");
            dialog.DismissAsync();
            Page.Dialog -= Page_Dialog2_EventHandler;
        }
        Page.Dialog += Page_Dialog2_EventHandler;
        await Page.GetByText("Next").ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).ClickAsync();
        await Page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("test@test.com");
        await Page.GetByText("Next").ClickAsync();
        await Page.GetByText("I Am In academia").ClickAsync();
        //Past that point, the recorder doesn't recognise the stopping event and requires more manual intervention
    }
    
    [TestMethod]
    public async Task ContactFormTest()
    {
        Page.SetDefaultTimeout(5000); // skip waiting 30sec for fail 
        await Page.GotoAsync("https://www.rosetic.ai/");
        await Page.GetByRole(AriaRole.Button, new() { Name = "connect" }).ClickAsync();
        await Page.GetByRole(AriaRole.Link, new() { Name = "contact us", Exact = true }).ClickAsync();
        await Page.GetByLabel("Contact form textbox", new() { Exact = true }).ClickAsync(); //simulates the failure observed during manual execution
    }
}
