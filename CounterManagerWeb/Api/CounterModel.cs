using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerWeb.Api {
    public record class CounterModel {
        public string? Name { get; set; }
        public int? Count { get; set; }

        public CounterModel()
        {
        }
    }
}
