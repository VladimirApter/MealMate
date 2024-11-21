using System.Drawing.Imaging;
using System.IO;
using Domain.DataBaseAccess;
using Domain.Logic;
using Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеры
builder.Services.AddControllers();

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = "api_docs"; // Установка маршрут для Swagger UI
});

// Регистрация маршрутов на верхнем уровне
app.MapControllers();

var databaseFolder = "Database";
var menuItemImagesFolder = Path.Combine(databaseFolder, "MenuItemImages");
if (!Directory.Exists(databaseFolder))
    Directory.CreateDirectory(databaseFolder);
if (!Directory.Exists(menuItemImagesFolder))
    Directory.CreateDirectory(menuItemImagesFolder);

using var context = new ApplicationDbContext();
context.Database.EnsureCreated();



app.Run();