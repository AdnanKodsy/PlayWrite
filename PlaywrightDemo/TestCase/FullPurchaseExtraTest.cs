using NUnit.Framework;
using PlaywrightSauceDemo.Drivers;
using PlaywrightSauceDemo.Pages;
using System.Threading.Tasks;
using Microsoft.Playwright;
using PlaywrightSauceDemo.Utilities;
using NUnit.Framework.Legacy;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;

[Parallelizable(ParallelScope.All)]
public class FullPurchaseExtraTest
{
    private IPage _page;
    private LoginPage _loginPage;
    private InventoryPage _inventoryPage;
    private CartPage _cartPage;
    private CheckoutPage _checkoutPage;
    private ConfirmationPage _confirmationPage;

    [SetUp]
    public async Task Setup()
    {

        _page = await PlaywrightDriver.InitializeAsync();
        _loginPage = new LoginPage(_page);
        _inventoryPage = new InventoryPage(_page);
        _cartPage = new CartPage(_page);
        _confirmationPage = new ConfirmationPage(_page);
        _checkoutPage = new CheckoutPage(_page);
        await _loginPage.NavigateAsync();
        await _loginPage.LoginAsync(TestConstants.StandardUser, TestConstants.Password);
    }
    [Test]
    public async Task FullPurchaseFlow_With_Add_Remove_Checkout()
    {

        await _inventoryPage.GetAddToCartButtonByProductName(TestConstants.Item1).ClickAsync();
        await _inventoryPage.GetAddToCartButtonByProductName(TestConstants.Item2).ClickAsync();
        await _inventoryPage.GetAddToCartButtonByProductName(TestConstants.Item3).ClickAsync();
        await _inventoryPage.GoToCartAsync();
        var CartItems = await _cartPage.GetCartItemNamesAsync();
        CollectionAssert.Contains(CartItems, TestConstants.Item1);
        CollectionAssert.Contains(CartItems, TestConstants.Item2);
        CollectionAssert.Contains(CartItems, TestConstants.Item3);

        await _cartPage.RemoveButton(TestConstants.Item1).ClickAsync();
        await _cartPage.RemoveButton(TestConstants.Item2).ClickAsync();

        await _cartPage.ClickCheckoutAsync();
        await _checkoutPage.FillCheckoutFormAsync(TestConstants.FName, TestConstants.LName, TestConstants.Zip);
        await _checkoutPage.FinishAsync();
        string ConfirmationM = await _confirmationPage.GetConfirmationTextAsync();

        Assert.That(ConfirmationM, Is.EqualTo(TestConstants.ConfirmationMessage));
    }


    [TearDown]
    public async Task TearDown()
    {
        if (_page?.Context != null)
        {
            var traceDir = Path.Combine("TestResults", "traces");
            Directory.CreateDirectory(traceDir);

            var traceFile = $"trace-{TestContext.CurrentContext.Test.Name}-{DateTime.Now:HHmmss}.zip";
            var tracePath = Path.Combine(traceDir, traceFile);

            await _page.Context.Tracing.StopAsync(new()
            {
                Path = tracePath
            });

            Console.WriteLine($"Trace saved: {tracePath}");

            if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                var screenshotPath = Path.Combine("TestResults", "screenshots", $"failure-{DateTime.Now:HHmmss}.png");
                Directory.CreateDirectory(Path.GetDirectoryName(screenshotPath));
                await _page.ScreenshotAsync(new() { Path = screenshotPath });
                Console.WriteLine($"Screenshot saved: {screenshotPath}");
            }
        }

        await _page.Context.Browser.CloseAsync();
    }

}