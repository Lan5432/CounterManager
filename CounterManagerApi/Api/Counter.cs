using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerApi.Api {
    public record struct Counter {

        public long Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
