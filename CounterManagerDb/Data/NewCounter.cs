using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerDb.Data {
    public record struct CounterModel {

        public string Name { get; set; }
        public int Count { get; set; }
    }
}
