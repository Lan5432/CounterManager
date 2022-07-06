namespace CounterManagerApi.Api {
    public static class UrlConstants {

        private const string BASE_URL = "api/";

        public const string CounterApiUrl = BASE_URL + "Counter";
        public static Func<string, string> CounterApiIdUrl = id => $"{CounterApiUrl}/{id}";
    }
}
