namespace CounterManagerApi.Api {
    public interface ICounterApi {

        Task<IList<Counter>> GetAllCounters();

        Task<Counter> GetCounter(long id);

        Task<Counter> CreateCounter(CounterModel request);

        Task<Counter> UpdateCounter(long id, CounterModel counter);

        Task DeleteCounter(long id);
    }
}
