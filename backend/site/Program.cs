using host.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using site;

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

var restaurant = new Restaurant(null, "ShaurmaEKB", "fonvizina", null, null, 2, 1, 1);
restaurant.NotificationGetter = new NotificationGetter("yura", null);
var listCategory = new List<Category>()
    { new Category("sok", 2, 1, new List<Dish>() { new Dish(null, 20, 100, "sopli", "vovini", 5, 1) }) };
restaurant.Menu = new Menu(null, listCategory, 2);
restaurant.Tables = new List<Table>() { new Table(5, null, 4) };
var restaurantApi = new ApiClient<Restaurant>();
restaurantApi.Post(restaurant);

//var menuApi = new ApiClient<Menu>();
//menuApi.Post(restaurant.Menu);

app.Run();