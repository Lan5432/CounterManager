using Microsoft.AspNetCore.WebUtilities;

namespace CounterManagerWeb2.Api {
    public class CounterRestApi : ICounterApi {

        private readonly HttpClient httpClient;

        public CounterRestApi(HttpClient httpClient) => this.httpClient = httpClient;

        public async Task<IList<Counter>> GetAllCounters()
        {
            var counter = await httpClient.GetFromJsonAsync<IList<Counter>>(UrlConstants.CounterApiUrl);

            return counter ?? new List<Counter>();
        }

        public async Task<Counter> GetCounter(long id)
        {
            var response = await httpClient.GetAsync(UrlConstants.CounterApiIdUrl(id));
            if (!response.IsSuccessStatusCode) {
                throw new ArgumentException($"Counter not found with id: {id}");
            }
            return await response.Content.ReadFromJsonAsync<Counter>();
        }

        public async Task<Counter> CreateCounter(CounterRequest request)
        {
            var httpResponse = await httpClient.PostAsJsonAsync(UrlConstants.CounterApiUrl, request);
            return await httpResponse.Content.ReadFromJsonAsync<Counter>();
        }
        public async Task<Counter> UpdateCounter(long id, CounterRequest counter)
        {
            var response = await httpClient.PutAsJsonAsync(UrlConstants.CounterApiIdUrl(id), counter);
            if (!response.IsSuccessStatusCode) {
                throw new ArgumentException($"Counter not found with id: {id}");
            }
            return await response.Content.ReadFromJsonAsync<Counter>();
        }

        public async Task DeleteCounter(long id)
        {
            var httpResponse = await httpClient.DeleteAsync(UrlConstants.CounterApiIdUrl(id));
            if (!httpResponse.IsSuccessStatusCode) {
                throw new ArgumentException($"Counter not found with id: {id}");
            }
        }

    }
}
