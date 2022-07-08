using Microsoft.EntityFrameworkCore;
using CounterManagerDb.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<CounterManagerDbContext>(options => {
        var connectionString = builder.Configuration.GetConnectionString("CounterManagerDbContext")
            ?? throw new InvalidOperationException("Connection string 'CounterManagerDbContext' not found.");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
