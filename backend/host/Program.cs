using System.IO;
using host.DataBaseAccess;
using host.Models;
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
if (!Directory.Exists(databaseFolder))
{
    Directory.CreateDirectory(databaseFolder);
}

/*if (!File.Exists(DataBaseAccess.PathDataBase))
{
    TestDataBase.CreateDataBase();
    //TestDataBase.AddDataToDataBase();
}*/

using var context = new ApplicationDbContext();
// Создаем базу данных и таблицу, если они не существуют
context.Database.EnsureCreated();

// Добавляем продукт
/*var dish = new Dish(null, 20, 200, "puk", "vovini", 2, 1);
context.Dishes.Add(dish);


var dish2 = new Dish(null, 20, 200, "kak", "vovini", 2, 1);
context.Dishes.Add(dish2);


var category = new Category("first", 1, 2, new List<Dish>(){dish, dish2});
context.Categories.Add(category);
context.SaveChanges();*/
/*// Выводим все продукты из базы данных
var products = context.Products.ToList();
Console.WriteLine("Products in database:");
foreach (var p in products)
{
    Console.WriteLine($"ID: {p.Id}, Name: {p.Name}, Price: {p.Price}");
}*/

app.Run();