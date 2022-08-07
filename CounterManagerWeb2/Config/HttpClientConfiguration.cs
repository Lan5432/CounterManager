
using CounterManagerWeb2.Api;

namespace CounterManagerWeb2.Config {
    public static class HttpClientConfiguration {

        public static IHttpClientBuilder AddRestClients(this IServiceCollection services, Action<HttpClient> configureClient) =>
                services.AddHttpClient<ICounterApi, CounterRestApi>((httpClient) => {
                    ApiClientFactory.ConfigureBasicHeaders(httpClient);
                    configureClient(httpClient);
                });
    }
}
