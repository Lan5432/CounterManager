using System.ComponentModel.DataAnnotations.Schema;

namespace CounterManagerWeb.Api {
    public record struct Counter(long Id, string Name, int Count) {

    }
}
