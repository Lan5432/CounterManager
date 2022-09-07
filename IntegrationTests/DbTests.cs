using CounterManagerDb.Data;
using IntegrationTests.Factories;
using System.Net;

namespace IntegrationTests {
    public class DbTests : IClassFixture<CounterManagerDbApplicationFactory>, IDisposable {

        private readonly CounterManagerDbApplicationFactory _factory;

        private readonly HttpClient _client;

        private readonly static string BASE_URL = "api/counter";
        private readonly Func<long, string> ID_URL = id => $"{BASE_URL}/{id}";

        public DbTests(CounterManagerDbApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCounters_ReturnsNone()
        {
            var response = await _client.GetAsync(BASE_URL);

            Assert.NotNull(response);
            var counterList = await response.Content.ReadFromJsonAsync<IList<Counter>>();
            Assert.Empty(counterList);
        }

        [Fact]
        public async Task GetAll_ReturnsCounter()
        {
            var counterName = "Test";
            var counterCount = 10;
            InitializeDb(new Counter(counterName, counterCount));

            var response = await _client.GetAsync(BASE_URL);

            Assert.NotNull(response);
            var counterList = await response.Content.ReadFromJsonAsync<IList<Counter>>();
            Assert.False(counterList.Count == 0);
            var existingCounter = counterList[0];
            Assert.Equal(counterName, existingCounter.Name);
            Assert.Equal(counterCount, existingCounter.Count);
        }


        [Fact]
        public async Task GetById_ReturnsCounter()
        {
            var counterName = "Test";
            var counterCount = 10;
            InitializeDb(new Counter(counterName, counterCount));

            var response = await _client.GetAsync(ID_URL(GetLastCounterId()));

            Assert.NotNull(response);
            var existingCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(counterName, existingCounter.Name);
            Assert.Equal(counterCount, existingCounter.Count);
        }

        [Fact]
        public async Task PostCounter_CreatesAndReturns()
        {
            var counterName = "Test";
            var initialCount = 10;

            var response = await _client.PostAsJsonAsync(BASE_URL, new Counter(counterName, initialCount));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var newCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(counterName, newCounter.Name);
            Assert.Equal(initialCount, newCounter.Count);
        }

        [Fact]
        public async Task PutCounter_Update()
        {
            var counterName = "Test";
            var counterCount = 10;
            InitializeDb(new Counter(counterName, counterCount));
            var changedName = "Test changed";
            var changedCount = 20;

            var response = await _client.PutAsJsonAsync(ID_URL(GetLastCounterId()), new Counter(changedName, changedCount));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Accepted, response.StatusCode);
            var changedCounter = await response.Content.ReadFromJsonAsync<Counter>();
            Assert.Equal(changedName, changedCounter.Name);
            Assert.Equal(changedCount, changedCounter.Count);
        }

        [Fact]
        public async Task DeleteCounter_IsRemoved()
        {
            var counterName = "Test";
            var counterCount = 10;
            InitializeDb(new Counter(counterName, counterCount));

            var response = await _client.DeleteAsync(ID_URL(GetLastCounterId()));

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var checkingCounterAgainResponse = await _client.GetAsync(ID_URL(1));
            Assert.Equal(HttpStatusCode.NotFound, checkingCounterAgainResponse.StatusCode);
        }

        private void InitializeDb(params Counter[] counters)
        {
            UseDbContext<CounterManagerDbContext>(db => {
                foreach (var counter in counters) {
                    db.Add(counter);
                }
                db.SaveChanges();
            });            
        }

        private long GetLastCounterId()
        {
            long lastCounterId = 0;
            UseDbContext<CounterManagerDbContext>(db => {
                lastCounterId = db.Counter.Max(counter => counter.Id);
            });
            return lastCounterId;
        }

        private void UseDbContext<T>(Action<T> operationOnDb)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope()) {
                var myDataContext = scope.ServiceProvider.GetService<T>();
                operationOnDb(myDataContext);
            }
        }

        public void Dispose()
        {
            _client.Dispose();
            UseDbContext<CounterManagerDbContext>(db => { 
                db.RemoveRange(db.Counter);
                db.SaveChanges();
            });
        }
    }
}