using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CounterManagerDb.Data {
    public class CounterManagerDbContext : DbContext
    {
        public CounterManagerDbContext (DbContextOptions<CounterManagerDbContext> options)
            : base(options)
        {
        }

        public DbSet<Counter>? Counter { get; set; }
    }
}
