using MusicSmash.API.Services;
using MusicSmash.API.Services.Implementations;
using MusicSmash.Database.Interfaces;
using MusicSmash.PostgreSQL.Implemenations;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging(config =>
{
    config.AddDebug();
    config.AddConsole();
});

builder.Services.AddScoped<IRoundService, RoundService>();

builder.Services.AddScoped<IConnection>(ctx => ConnectionFactory.GetConnection(configuration["db:connection-string"]));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
