using Domain.Logic;
using Domain.Models;
using Microsoft.Extensions.FileProviders;
using site;

var builder = WebApplication.CreateBuilder(args);


// Добавление контроллеры
builder.Services.AddControllersWithViews();

// Добавление Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = "api_docs"; // Установка маршрут для Swagger UI
});

// Регистрация маршрутов на верхнем уровне
app.MapControllers();


//ресторан 1
// Создаем объекты Nutrients для блюд и напитков
var pastaNutrients = new Nutrients(
    id: null,
    //menuItemId: 1,
    kilocalories: 520,
    proteins: 15,
    fats: 12,
    carbohydrates: 75
);

var pizzaNutrients = new Nutrients(
    id: null,
    //menuItemId: 2,
    kilocalories: 680,
    proteins: 20,
    fats: 20,
    carbohydrates: 90
);

var wineNutrients = new Nutrients(
    id: null,
    //menuItemId: 3,
    kilocalories: 150,
    proteins: 0,
    fats: 0,
    carbohydrates: 5
);

// Создаем объекты блюд (Dish) и напитков (Drink)
var spaghettiCarbonara = new Dish(
    id: 229,
    categoryId: 1,
    cookingTimeMinutes: 15,
    price: 12.99,
    weight: 228,
    name: "Спагетти Карбонара",
    description: "Классическое итальянское блюдо с беконом, яйцами и сыром.",
    imagePath: "1.jpeg",
    nutrients: pastaNutrients
);
var dishApi = new ApiClient<Dish>();
dishApi.Post(spaghettiCarbonara);


app.Run();