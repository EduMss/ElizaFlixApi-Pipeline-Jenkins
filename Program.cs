using ElizaFlixAPI.Interfaces;
using ElizaFlixAPI.Infra;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adicione configurações do arquivo JSON
builder.Configuration.AddJsonFile("/config/config.json", optional: false, reloadOnChange: true);

builder.Services.AddTransient<IFilmeRepository, FilmeRepository>();

var app = builder.Build();

// Configuração da política CORS
app.UseCors(builder =>
{
    builder.WithOrigins("http://192.168.0.249:3001")
           .AllowAnyHeader()
           .AllowAnyMethod()
           .AllowAnyOrigin();

    builder.WithOrigins("http://192.168.0.109:8080")
    .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();

    builder.WithOrigins("http://192.168.0.109:3001")
    .AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

//error Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
