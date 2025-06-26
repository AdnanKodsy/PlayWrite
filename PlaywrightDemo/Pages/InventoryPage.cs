using Microsoft.Playwright;
using PlaywrightSauceDemo.Utilities;

namespace PlaywrightSauceDemo.Pages
{
    public class InventoryPage
    {
        private readonly IPage _page;

        public InventoryPage(IPage page) => _page = page;

        private ILocator ProductsTitle => _page.Locator(TestConstants.Header);
        private ILocator Item => _page.Locator("inventory-item");

        public async Task<string> GetTitleAsync()
        {
            return await ProductsTitle.InnerTextAsync();
        }

        public ILocator GetAddToCartButtonByProductName(string productName)
        {
        return _page.Locator(".inventory_item").Filter(new LocatorFilterOptions {HasTextString = productName }).Locator("button");
        }   

        public async Task GoToCartAsync() => await _page.ClickAsync(".shopping_cart_link");

    }
}
