using Microsoft.Playwright;
using System.Threading.Tasks;

namespace PlaywrightSauceDemo.Drivers
{
    public static class PlaywrightDriver
    {
        public static IPlaywright PlaywrightInstance { get; private set; }
        public static IBrowser Browser { get; private set; }

        public static async Task<IPage> InitializeAsync()
        {
            PlaywrightInstance = await Playwright.CreateAsync();
            Browser = await PlaywrightInstance.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 500
            }
            );

            var context = await Browser.NewContextAsync(new()
            {
                RecordVideoDir = Path.Combine(Directory.GetCurrentDirectory(), "videos"),
                RecordVideoSize = new() { Width = 1280, Height = 720 }
            });
            await context.Tracing.StartAsync(new() { Screenshots = true, Snapshots = true, Sources = true});

            return await context.NewPageAsync();
        }


        public static async Task CleanupAsync()
        {
            await Browser.CloseAsync();
            PlaywrightInstance.Dispose();
        }
    }
}
