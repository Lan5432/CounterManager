using CounterManagerDb;
using CounterManagerDb.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Factories {
    public class CounterManagerDbApplicationFactory : WebApplicationFactory<Program> {

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services
                    .SingleOrDefault(d => d.ServiceType ==typeof(DbContextOptions<CounterManagerDbContext>));

                if(descriptor != null) {
                    services.Remove(descriptor);
                }

                services.AddDbContext<CounterManagerDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var serviceProvider = services.BuildServiceProvider();

                using var scope = serviceProvider.CreateScope();
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<CounterManagerDbContext>();

                db.Database.EnsureCreated();
            });
        }
    }
}
