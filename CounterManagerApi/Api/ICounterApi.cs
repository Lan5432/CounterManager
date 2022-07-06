namespace CounterManagerApi.Api {
    public interface ICounterApi {

        Task<IList<Counter>> GetAllCounters();

        Task<Counter> GetCounter(string id);

        Task<Counter> CreateCounter(CounterRequest request);

        Task<Counter> UpdateCounter(string id, CounterRequest request);

        Task DeleteCounter(string id);
    }
}
