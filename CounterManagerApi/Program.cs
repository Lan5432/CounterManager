using CounterManagerApi.Config;
using CounterManagerApi.Controllers;
var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.

services.AddControllers();
services.AddCounterDbApiClient(httpClient => {
    var url = $"{builder.Configuration["Apis:CounterDb:Host"]}:{builder.Configuration["Apis:CounterDb:Port"]}";
    httpClient.BaseAddress = new(url);
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName.Equals("Dockerized")) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
