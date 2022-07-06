using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerApi.Api {
    public record struct Counter {

        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
