using CSharpBasicSKA2.Controllers;
using CSharpBasicSKA2.Models;
using CSharpBasicSKA2.Proxies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IpProxy>();
builder.Services.AddSingleton<CountryCodeProxy>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<List<CountryCodeRequestModel>>();
// builder.Services.AddSingleton<ILogger<WhereAmIController>>();


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