using System.Net.Mime;

namespace CounterManagerWeb.Config {
    public static class ApiClientFactory {

        internal static void ConfigureHttpClient(HttpClient client)
        {
            ConfigureBasicHeaders(client);
        }

        internal static void ConfigureBasicHeaders(HttpClient client)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new(MediaTypeNames.Application.Json));
        }
    }
}
