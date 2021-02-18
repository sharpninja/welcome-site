using System.Threading.Tasks;

using Microsoft.JSInterop;

namespace WelcomeSite
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;

        public BrowserService(IJSRuntime js)
        {
            _js = js;
        }

#pragma warning disable AsyncFixer01 // Unnecessary async/await usage
        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }
#pragma warning restore AsyncFixer01 // Unnecessary async/await usage

    }
}
