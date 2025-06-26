using Microsoft.Playwright;

public class ConfirmationPage
{
    private readonly IPage _page;
    public ConfirmationPage(IPage page) => _page = page;

    public async Task<string> GetConfirmationTextAsync() => await _page.Locator(".complete-header").InnerTextAsync();
}
