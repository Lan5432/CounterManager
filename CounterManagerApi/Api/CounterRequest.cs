using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerApi.Api {
    public record struct CounterRequest {

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
