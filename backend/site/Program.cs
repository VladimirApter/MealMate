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


/*var dish = new Dish(1488, 20, 200, "dd", "vovini", 2, 3);
var dishApi = new ApiClient<Dish>();
dishApi.Post(dish);*/
var dish = new Dish(1, 20, 200, "puk", "vovini", 2, 1);


var dish2 = new Dish(null, 20, 200, "kak", "vovini", 2, 1);

var category = new Category("first", 2, 2, new List<Dish>(){dish, dish2});
var categoryApi = new ApiClient<Category>();
categoryApi.Post(category);

//var menuApi = new ApiClient<Menu>();
//menuApi.Post(restaurant.Menu);

app.Run();