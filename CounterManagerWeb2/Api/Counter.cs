using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerWeb2.Api {
    public record struct Counter {

        public long Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }
}
