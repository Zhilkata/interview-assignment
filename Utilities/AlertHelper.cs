using Microsoft.Playwright;

namespace InterviewTask.Utilities;

public static class AlertHelper
{
    public static void SetupAlertHandler(IPage page, Action<string> onDialogMessage)
    {
        page.Dialog += async (_, dialog) =>
        {
            onDialogMessage?.Invoke(dialog.Message);
            await dialog.DismissAsync();
        };
    }
}