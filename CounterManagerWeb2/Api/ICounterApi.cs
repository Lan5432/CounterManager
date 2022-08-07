namespace CounterManagerWeb2.Api {
    public interface ICounterApi {

        Task<IList<Counter>> GetAllCounters();

        Task<Counter> GetCounter(long id);

        Task<Counter> CreateCounter(CounterRequest request);

        Task<Counter> UpdateCounter(long id, CounterRequest counter);

        Task DeleteCounter(long id);
    }
}
