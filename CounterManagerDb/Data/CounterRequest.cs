using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerDb.Data {
    public record struct CounterRequest {

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
