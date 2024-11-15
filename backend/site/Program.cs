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


var dish = new Dish(null, 20, "sopli", "vovini", 10, 2, 3);
var dishApi = new ApiClient<Dish>();
dishApi.Post(dish);

var notificationGetter = new NotificationGetter("vova", 2, 4);
var notificationGetterApi = new ApiClient<NotificationGetter>();
notificationGetterApi.Post(notificationGetter);

var owner = new Owner("yura", 1, new List<int>(){1, 2, 3});
var ownerApi = new ApiClient<Owner>();
ownerApi.Post(owner);
//var owner2 = ownerApi.Get(1);

var restaurant = new Restaurant(null, "ShaurmaEKB", "fonvizina", null, null, 2, 1);
restaurant.NotificationGetter = new NotificationGetter("yura", null, 2);
var listCategory = new List<Category>()
    { new Category("sok", 2, 1, new List<Dish>() { new Dish(null, 20, "sopli", "vovini", 20, 2, 10) }) };
restaurant.Menu = new Menu(null, listCategory, 2);
restaurant.Tables = new List<Table>() { new Table(5, null, 4) };
var restaurantApi = new ApiClient<Restaurant>();
restaurantApi.Post(restaurant);

var dish3 = new Dish(1, 20, "puk", "vovini", 200, 2, 1);


var dish2 = new Dish(null, 20, "kak", "vovini", 5, 2, 1);

var category = new Category("first", 2, 2, new List<Dish>(){dish3, dish2});
var categoryApi = new ApiClient<Category>();
categoryApi.Post(category);

var menuApi = new ApiClient<Menu>();
menuApi.Post(restaurant.Menu);

app.Run();