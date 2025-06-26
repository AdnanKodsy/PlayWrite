using Microsoft.Playwright;

public class CheckoutPage
{
    private readonly IPage _page;
    public CheckoutPage(IPage page) => _page = page;

    public async Task FillCheckoutFormAsync(string firstName, string lastName, string zip)
    {
        await _page.FillAsync("#first-name", firstName);
        await _page.FillAsync("#last-name", lastName);
        await _page.FillAsync("#postal-code", zip);
        await _page.ClickAsync("#continue");
    }

    public async Task FinishAsync() => await _page.ClickAsync("#finish");
}