using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerDb.Data {
    public record struct CounterModel(string Name, int Count) {
    }
}
