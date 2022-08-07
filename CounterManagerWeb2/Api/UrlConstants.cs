namespace CounterManagerWeb2.Api {
    public static class UrlConstants {

        private const string BASE_URL = "api/";

        public const string CounterApiUrl = BASE_URL + "Counter";
        public static Func<long, string> CounterApiIdUrl = id => $"{CounterApiUrl}/{id}";
    }
}
