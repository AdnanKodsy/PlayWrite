using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightSauceDemo.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;

        public LoginPage(IPage page) => _page = page;

        private ILocator UsernameInput => _page.Locator("#user-name");
        private ILocator PasswordInput => _page.Locator("#password");
        private ILocator LoginButton => _page.Locator("#login-button");

        public async Task NavigateAsync()
        {
            await _page.GotoAsync("https://www.saucedemo.com/");
        }

        public async Task LoginAsync(string username, string password)
        {
            await UsernameInput.FillAsync(username);
            await PasswordInput.FillAsync(password);
            await LoginButton.ClickAsync();
        }
    }
}
