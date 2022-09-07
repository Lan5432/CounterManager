using CounterManagerWeb.Api;
using IntegrationTests.Factories;
using Microsoft.Playwright;
using Newtonsoft.Json;

namespace IntegrationTests {
    public class WebTests : IClassFixture<CounterManagerWebApplicationFactory>, IDisposable, IAsyncDisposable {

        private readonly CounterManagerWebApplicationFactory _factory;

        private readonly MockServer _server;
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private readonly HttpClient _client;

        private readonly static string API_BASE_URL = "/api/counter";
        private readonly Func<long, string> API_ID_URL = id => $"{API_BASE_URL}/{id}";

        //https://playwright.dev/dotnet/docs/test-assertions
        public WebTests(CounterManagerWebApplicationFactory factory)
        {
            _factory = factory;

            _server = new MockServer(8082);
        }

        private async Task SetupPlaywright()
        {
            if(_playwright == null) {
                _playwright = await Playwright.CreateAsync();
                _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = true
                });
            }
            _page = await _browser.NewPageAsync();
        }

        [Fact]
        public async void HomePage_ShowsNewCounterForm()
        {
            await SetupPlaywright();
            _server.MockResponse(API_BASE_URL, new List<Counter>(), HttpMethod.Get);

            var response = await _page.GotoAsync(_factory.ServerAddress);

            Assert.NotNull(response);
            Assert.True(await _page.Locator("#new-counter").IsVisibleAsync());
        }

        [Fact]
        public async void HomePage_GivenCountersExist_ShowsUpdateCounterForm()
        {
            await SetupPlaywright();
            _server.MockResponse(API_BASE_URL, new List<Counter> { new Counter(1, "Test", 20) }, HttpMethod.Get);

            var response = await _page.GotoAsync(_factory.ServerAddress);

            Assert.NotNull(response);
            Assert.True(await _page.Locator("#stored-counters").IsVisibleAsync());
        }

        [Fact]
        public async void HomePage_CanCreateCounter()
        {
            await SetupPlaywright();

            var counterName = "Test counter created";
            var counterCount = 25;
            _server.MockResponse(API_BASE_URL, new List<Counter>(), HttpMethod.Get);
            _server.MockResponse(API_BASE_URL, new Counter(1, counterName, counterCount), HttpMethod.Post);

            await _page.GotoAsync(_factory.ServerAddress);
            await _page.Locator("#create-counter-name").FillAsync(counterName);
            await _page.Locator("#create-counter-count").FillAsync(counterCount.ToString());

            _server.MockResponse(API_BASE_URL, new List<Counter> { new Counter(1, counterName, counterCount) }, HttpMethod.Get);
            await _page.Locator("#create-counter-submit").ClickAsync();

            var requestToApi = _server.GetRequestEntry(API_BASE_URL, HttpMethod.Post);
            var counterBodyPostedToApi = JsonConvert.DeserializeObject<CounterModel>(requestToApi.Body);
            Assert.Equal(API_BASE_URL, requestToApi.Path);
            Assert.Equal(counterName, counterBodyPostedToApi.Name);
            Assert.Equal(counterCount, counterBodyPostedToApi.Count);

            Assert.Equal(counterName, await _page.Locator("#modify-counter-1-name").InputValueAsync());
            Assert.Equal(counterCount.ToString(), await _page.Locator("#modify-counter-1-count").InputValueAsync());
        }

        [Fact]
        public async void HomePage_CanUpdateCounter()
        {
            await SetupPlaywright();

            var counterIdToModify = 2;
            var newCounterName = "Test counter created";
            var newCount = 35;
            _server.MockResponse(API_BASE_URL
                , new List<Counter> { new Counter(1, "Test", 20), new Counter(counterIdToModify, "Test2", 25) }
                , HttpMethod.Get);

            await _page.GotoAsync(_factory.ServerAddress);
            await _page.Locator($"#modify-counter-{counterIdToModify}-name").FillAsync(newCounterName);
            await _page.Locator($"#modify-counter-{counterIdToModify}-count").FillAsync(newCount.ToString());

            _server.MockResponse(API_BASE_URL 
                , new List<Counter> { new Counter(1, "Test", 20), new Counter(counterIdToModify, newCounterName, newCount) }
                , HttpMethod.Get);
            _server.MockResponse(API_ID_URL.Invoke(counterIdToModify)
                , new Counter(counterIdToModify, newCounterName, newCount)
                , HttpMethod.Put);
            await _page.Locator($"#modify-counter-{counterIdToModify}-update").ClickAsync();

            var updateRequestToApi = _server.GetRequestEntry(API_ID_URL.Invoke(counterIdToModify), HttpMethod.Put);
            var counterBodyPutToApi = JsonConvert.DeserializeObject<CounterModel>(updateRequestToApi.Body);
            Assert.Equal(API_ID_URL.Invoke(counterIdToModify), updateRequestToApi.Path);
            Assert.Equal(newCounterName, counterBodyPutToApi.Name);
            Assert.Equal(newCount, counterBodyPutToApi.Count);

            Assert.Equal(newCounterName, await _page.Locator($"#modify-counter-{counterIdToModify}-name").InputValueAsync());
            Assert.Equal(newCount.ToString(), await _page.Locator($"#modify-counter-{counterIdToModify}-count").InputValueAsync());
        }

        [Fact]
        public async void HomePage_CanDeleteCounter()
        {
            await SetupPlaywright();

            _server.MockResponse(API_BASE_URL, new List<Counter> { new Counter(1, "Test", 20) }, HttpMethod.Get);

            await _page.GotoAsync(_factory.ServerAddress);
            await _page.Locator("#modify-counter-1-delete").ClickAsync();

            var requestToApi = _server.GetRequestEntry(API_BASE_URL, HttpMethod.Delete);
            Assert.Equal(API_ID_URL.Invoke(1), requestToApi.Path);
        }

        public void Dispose()
        {
            _server.Dispose();
            _playwright.Dispose();
            _browser.DisposeAsync();
        }

        public async ValueTask DisposeAsync()
        {
            _client.Dispose();
            _server.Dispose();
            _playwright.Dispose();
            await _browser.DisposeAsync();
        }
    }
}
