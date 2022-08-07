namespace CounterManagerApi.Api {
    public static class UrlConstants {

        private const string BASE_URL = "api/";

        public const string CounterApiUrl = BASE_URL + "counter";

        public static Func<long, string> CounterApiUrlById = id => $"{CounterApiUrl}/{id}";
    }
}
