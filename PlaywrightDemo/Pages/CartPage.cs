using Microsoft.Playwright;

public class CartPage
{
    private readonly IPage _page;
    public CartPage(IPage page) => _page = page;

    public ILocator RemoveButton(string productName)
        => _page.Locator(".cart_item").Filter(new() { HasTextString = productName }).Locator("button");

    public async Task ClickCheckoutAsync() => await _page.ClickAsync("#checkout");
    public async Task<List<string>> GetCartItemNamesAsync()
{
    var items = _page.Locator(".cart_item .inventory_item_name");
    int count = await items.CountAsync();
    var names = new List<string>();

    for (int i = 0; i < count; i++)
    {
        names.Add(await items.Nth(i).InnerTextAsync());
    }

    return names;
}
}