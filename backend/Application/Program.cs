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

var databasePath = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, "..", "Domain", "Database", "MenuItemImages"));
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(databasePath),
    RequestPath = "/Domain/Database/MenuItemImages"
});

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
    menuItemId: 1,
    kilocalories: 520,
    proteins: 15,
    fats: 12,
    carbohydrates: 75
);

var pizzaNutrients = new Nutrients(
    id: null,
    menuItemId: 2,
    kilocalories: 680,
    proteins: 20,
    fats: 20,
    carbohydrates: 90
);

var wineNutrients = new Nutrients(
    id: null,
    menuItemId: 3,
    kilocalories: 150,
    proteins: 0,
    fats: 0,
    carbohydrates: 5
);

// Создаем объекты блюд (Dish) и напитков (Drink)
var spaghettiCarbonara = new Dish(
    id: null,
    categoryId: 1,
    cookingTimeMinutes: 15,
    price: 12.99,
    weight: 350,
    name: "Спагетти Карбонара",
    description: "Классическое итальянское блюдо с беконом, яйцами и сыром.",
    imagePath: "/Domain/Database/MenuItemImages/1.jpeg",
    nutrients: pastaNutrients
);

var margheritaPizza = new Dish(
    id: null,
    categoryId: 1,
    cookingTimeMinutes: 20,
    price: 10.99,
    weight: 450,
    name: "Пицца Маргарита",
    description: "Традиционная пицца с томатным соусом, моцареллой и базиликом.",
    imagePath: "https://lh3.googleusercontent.com/-F7-f2RyixFJ_0-MIGehlz7lp08CkWuy7Y64qDx8zcSrAyHA_uWVnJx1XOVAHg_qoFD7fW34aWScKlOz7tlHx8LeBxDoB64vaZ6LCKKMAPPnr8-QTpPpQVVK-xGPWFZomSVkVZXW",
    nutrients: pizzaNutrients
);

var italianRedWine = new Drink(
    id: null,
    categoryId: 2,
    cookingTimeMinutes: 0,
    price: 7.50,
    volume: 150,
    name: "Итальянское красное вино",
    description: "Бокал изысканного красного вина из региона Тоскана.",
    imagePath: "https://www.klenmarket.ru/upload/shop_1/3/2/6/item_326671/shop_property_file_326671_525318.jpg",
    nutrients: wineNutrients
);

// Создаем категории меню
var dishesCategory = new Category(
    id: 1,
    menuId: 1,
    name: "Блюда",
    menuItems: new List<MenuItem> { spaghettiCarbonara, margheritaPizza }
);

var drinksCategory = new Category(
    id: 2,
    menuId: 1,
    name: "Напитки",
    menuItems: new List<MenuItem> { italianRedWine }
);

// Создаем меню ресторана
var menu = new Menu(
    id: 1,
    categories: new List<Category> { dishesCategory, drinksCategory },
    restaurantId: 1
);

// Создаем список столов в ресторане
var tables = new List<Table>
{
    new Table(id: 1, restaurantId: 1, number: 1),
    new Table(id: 2, restaurantId: 1, number: 2),
    new Table(id: 3, restaurantId: 1, number: 3)
};

Console.WriteLine(tables[0].Token);

// Задаем географические координаты ресторана
var coordinates = new GeoCoordinates(
    restaurantId: 1,
    latitude: 41.902782,    // Широта Рима
    longitude: 12.496366    // Долгота Рима
);

// Создаем объект NotificationGetter для уведомлений
var notificationGetter = new NotificationGetter(
    username: "manager_bellaitalia",
    id: null,
    restaurantId: 1
);

// Создаем объект ресторана
var restaurant = new Restaurant(
    id: 1,
    ownerId: 456,
    name: "La Bella Italia",
    coordinates: coordinates,
    notificationGetter: notificationGetter,
    menu: menu,
    tables: tables
);

// Отправляем ресторан через API-клиент
var restaurantApi = new ApiClient<Restaurant>();
restaurantApi.Post(restaurant);


//ресторан 2
// Создаем объекты Nutrients для блюд и напитков
var sushiNutrients = new Nutrients(
    id: null,
    menuItemId: 4,
    kilocalories: 300,
    proteins: 25,
    fats: 10,
    carbohydrates: 30
);

var ramenNutrients = new Nutrients(
    id: null,
    menuItemId: 5,
    kilocalories: 550,
    proteins: 20,
    fats: 15,
    carbohydrates: 70
);

var sakeNutrients = new Nutrients(
    id: null,
    menuItemId: 6,
    kilocalories: 120,
    proteins: 0,
    fats: 0,
    carbohydrates: 5
);

// Создаем объекты блюд (Dish) и напитков (Drink)
var californiaRoll = new Dish(
    id: null,
    categoryId: 3,
    cookingTimeMinutes: 10,
    price: 9.99,
    weight: 200,
    name: "Калифорнийский ролл",
    description: "Ролл с крабом, авокадо и икрой тобико.",
    imagePath: "images/california_roll.jpg",
    nutrients: sushiNutrients
);

var spicyRamen = new Dish(
    id: null,
    categoryId: 3,
    cookingTimeMinutes: 25,
    price: 14.99,
    weight: 400,
    name: "Острый Рамен",
    description: "Сытный рамен с пряным бульоном и кусочками свинины.",
    imagePath: "images/spicy_ramen.jpg",
    nutrients: ramenNutrients
);

var japaneseSake = new Drink(
    id: null,
    categoryId: 4,
    cookingTimeMinutes: 0,
    price: 8.50,
    volume: 180,
    name: "Японское саке",
    description: "Традиционный японский алкогольный напиток из риса.",
    imagePath: "images/japanese_sake.jpg",
    nutrients: sakeNutrients
);

// Создаем категории меню
var sushiCategory = new Category(
    id: 3,
    menuId: 2,
    name: "Суши и Роллы",
    menuItems: new List<MenuItem> { californiaRoll, spicyRamen }
);

var drinksCategory2 = new Category(
    id: 4,
    menuId: 2,
    name: "Напитки",
    menuItems: new List<MenuItem> { japaneseSake }
);

// Создаем меню ресторана
var menu2 = new Menu(
    id: 2,
    categories: new List<Category> { sushiCategory, drinksCategory2 },
    restaurantId: 2
);

// Создаем список столов в ресторане
var tables2 = new List<Table>
{
    new Table(id: 4, restaurantId: 2, number: 1),
    new Table(id: 5, restaurantId: 2, number: 2),
    new Table(id: 6, restaurantId: 2, number: 3)
};

// Задаем географические координаты ресторана
var coordinates2 = new GeoCoordinates(
    restaurantId: 2,
    latitude: 35.689487,    // Широта Токио
    longitude: 139.691711  // Долгота Токио
);

// Создаем объект NotificationGetter для уведомлений
var notificationGetter2 = new NotificationGetter(
    username: "manager_tokyosushi",
    id: null,
    restaurantId: 2
);

// Создаем объект ресторана
var restaurant2 = new Restaurant(
    id: 2,
    ownerId: 789,
    name: "Tokyo Sushi Bar",
    coordinates: coordinates2,
    notificationGetter: notificationGetter2,
    menu: menu2,
    tables: tables2
);

// Отправляем ресторан через API-клиент
var restaurantApi2 = new ApiClient<Restaurant>();
restaurantApi2.Post(restaurant2);


app.Run();