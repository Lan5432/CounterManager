using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerWeb2.Api {
    public record class CounterRequest {

        public string? Name { get; set; }
        public int? Count { get; set; }

        public CounterRequest()
        {
        }
    }
}
