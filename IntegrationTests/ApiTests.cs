using CounterManagerApi.Api;
using IntegrationTests.Factories;
using Newtonsoft.Json;
using System.Net;

namespace IntegrationTests {
    public class ApiTests : IClassFixture<CounterManagerApiApplicationFactory>, IDisposable {

        private readonly CounterManagerApiApplicationFactory _factory;

        private readonly HttpClient _client;

        private readonly MockServer _server;

        private readonly static string BASE_URL = "/api/counter";
        private readonly Func<long, string> ID_URL = id => $"{BASE_URL}/{id}";

        public ApiTests(CounterManagerApiApplicationFactory factory)
        {
            _factory = factory;

            _client = _factory.CreateClient();

            _server = new MockServer(8081);
        }

        [Fact]
        public async Task GetAllCounters_ReturnsSome()
        {
            var mockedCounters = new List<Counter> { new Counter(1, "Test", 20), new Counter(2, "Test2", 30) };
            _server.MockResponse(BASE_URL, mockedCounters, HttpMethod.Get);

            var response = await _client.GetAsync(BASE_URL);

            Assert.NotNull(response);
            var returnedCounters = await response.Content.ReadFromJsonAsync<List<Counter>>();
            Assert.Equal(2, returnedCounters.Count);
            Assert.Equal(mockedCounters[0], returnedCounters.Find(counter => counter.Id == 1));
        }

        [Fact]
        public async Task GetCounterById_ReturnsIt()
        {
            var counterId = 1;
            var mockedCounter = new Counter(counterId, "Test", 20);
            _server.MockResponse(ID_URL.Invoke(counterId), mockedCounter, HttpMethod.Get);

            var response = await _client.GetAsync(ID_URL.Invoke(counterId));

            Assert.NotNull(response);
            var returnedCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(mockedCounter, returnedCounter);

            var requestToDb = _server.GetRequestEntry(ID_URL.Invoke(counterId), HttpMethod.Get);
            Assert.Equal(ID_URL.Invoke(counterId), requestToDb.Path);
            Assert.Null(requestToDb.Body);
        }

        [Fact]
        public async Task PostCounter_IsCreated()
        {
            var createCounter = new CounterModel { Name = "Test", Count = 20 };
            var mockedCounter = new Counter(1, "Test", 20);
            _server.MockResponse(BASE_URL, mockedCounter, HttpMethod.Post);

            var response = await _client.PostAsJsonAsync(BASE_URL, createCounter);

            Assert.NotNull(response);
            var returnedCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(createCounter.Name, returnedCounter.Name);
            Assert.Equal(createCounter.Count, returnedCounter.Count);

            var requestToDb = _server.GetRequestEntry(BASE_URL, HttpMethod.Post);
            Assert.Equal(BASE_URL, requestToDb.Path);
            Assert.Equal(createCounter, JsonConvert.DeserializeObject<CounterModel>(requestToDb.Body));
        }

        [Fact]
        public async Task PutCounter_IsUpdated()
        {
            var counterId = 1;
            var updatedCounter = new CounterModel { Name = "Test", Count = 40 };
            var mockedCounter = new Counter(counterId, "Test", 40);
            _server.MockResponse(ID_URL.Invoke(counterId), mockedCounter, HttpMethod.Put, HttpStatusCode.Accepted);

            var response = await _client.PutAsJsonAsync(ID_URL.Invoke(counterId), updatedCounter);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            var returnedCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(updatedCounter.Name, returnedCounter.Name);
            Assert.Equal(updatedCounter.Count, returnedCounter.Count);

            var requestToDb = _server.GetRequestEntry(ID_URL.Invoke(counterId), HttpMethod.Put);
            Assert.Equal(ID_URL.Invoke(counterId), requestToDb.Path);
            Assert.Equal(updatedCounter, JsonConvert.DeserializeObject<CounterModel>(requestToDb.Body));
        }

        [Fact]
        public async Task DeleteCounter_IsDeleted()
        {
            _server.MockResponse(ID_URL.Invoke(1), "", HttpMethod.Delete, HttpStatusCode.NoContent);

            var response = await _client.DeleteAsync(ID_URL.Invoke(1));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal("", await response.Content.ReadAsStringAsync());
        }

        public void Dispose()
        {
            _client.Dispose();
            _server.Dispose();
        }
    }
}
