using System.Data.SQLite;
using host.Models;

namespace host.DataBaseAccess;

internal static class TestDataBase
{
    private const string BaseName = DataBaseAccess.PathDataBase;

    public static void CreateDataBase()
    {
        SQLiteConnection.CreateFile(BaseName);
        
        using var connection = new SQLiteConnection("Data Source=" + BaseName);
        connection.Open();
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [owners] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [name] char(100) NOT NULL
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [notification_getters] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [username] char(100) NOT NULL,
                    [restaurant_id] int NOT NULL
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [restaurants] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [owner_id] int NOT NULL,
                    [name] char(100) NOT NULL,
                    [address] char(200) NOT NULL,
                    FOREIGN KEY (owner_id) REFERENCES owners (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [menus] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [restaurant_id] int NOT NULL,
                    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [categories] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [menu_id] int NOT NULL,
                    [name] char(100) NOT NULL,
                    FOREIGN KEY (menu_id) REFERENCES menus (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            
            command.CommandText = @"CREATE TABLE [dishes] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [category_id] int NOT NULL,
                    [price] real NOT NULL,
                    [weight] real NOT NULL,
                    [name] char(100) NOT NULL,
                    [description] text NOT NULL,
                    [cooking_time_minutes] int NOT NULL,
                    FOREIGN KEY (category_id) REFERENCES categories (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [orders] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [client_id] int NOT NULL,
                    [restaurant_id] int NOT NULL,
                    [comment] text,
                    [datetime] text NOT NULL,
                    [status] integer NOT NULL,
                    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [order_items] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [order_id] int NOT NULL,
                    [dish_id] int NOT NULL,
                    [count] int NOT NULL,
                    [price] real NOT NULL,
                    FOREIGN KEY (order_id) REFERENCES orders (id),
                    FOREIGN KEY (dish_id) REFERENCES dishes (id)
                );";
            command.ExecuteNonQuery();
        }
        
        using (var command = new SQLiteCommand(connection))
        {
            command.CommandText = @"CREATE TABLE [tables] (
                    [id] integer PRIMARY KEY AUTOINCREMENT NOT NULL,
                    [restaurant_id] int NOT NULL,
                    [number] int NOT NULL,
                    FOREIGN KEY (restaurant_id) REFERENCES restaurants (id)
                );";
            command.ExecuteNonQuery();
        }

        Console.WriteLine("Таблицы успешно созданы");
        
    }

    public static void AddDataToDataBase()
    {
        using var connection = new SQLiteConnection("Data Source=" + BaseName);
        connection.Open();
        // Добавление данных в таблицу owners
        var owners = new[]
        {
            new { Name = "Иван Петров" },
            new { Name = "Мария Сидорова" }
        };

        foreach (var owner in owners)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [owners] (name) 
                                                  VALUES (@name);";

            insertCommand.Parameters.AddWithValue("@name", owner.Name);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу notification_getters
        var notificationGetters = new[]
        {
            new { UserName = "ivan@example.com", RestaurantId = 1 },
            new { UserName = "maria@example.com", RestaurantId = 2 }
        };

        foreach (var notificationGetter in notificationGetters)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [notification_getters] (username, restaurant_id) 
                                                  VALUES (@username, @restaurantId);";

            insertCommand.Parameters.AddWithValue("@username", notificationGetter.UserName);
            insertCommand.Parameters.AddWithValue("@restaurantId", notificationGetter.RestaurantId);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу restaurants
        var restaurants = new[]
        {
            new { OwnerId = 1, Name = "Ресторан А", Address = "Улица 1, дом 1" },
            new { OwnerId = 2, Name = "Ресторан Б", Address = "Улица 2, дом 2" }
        };

        foreach (var restaurant in restaurants)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [restaurants] (owner_id, name, address) 
                                                  VALUES (@ownerId, @name, @address);";

            insertCommand.Parameters.AddWithValue("@ownerId", restaurant.OwnerId);
            insertCommand.Parameters.AddWithValue("@name", restaurant.Name);
            insertCommand.Parameters.AddWithValue("@address", restaurant.Address);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу menus
        var menus = new[]
        {
            new { RestaurantId = 1 },
            new { RestaurantId = 2 }
        };

        foreach (var menu in menus)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [menus] (restaurant_id) 
                                                  VALUES (@restaurantId);";

            insertCommand.Parameters.AddWithValue("@restaurantId", menu.RestaurantId);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу categories
        var categories = new[]
        {
            new { MenuId = 1, Name = "Закуски" },
            new { MenuId = 1, Name = "Основные блюда" },
            new { MenuId = 2, Name = "Десерты" }
        };

        foreach (var category in categories)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [categories] (menu_id, name) 
                                                  VALUES (@menuId, @name);";

            insertCommand.Parameters.AddWithValue("@menuId", category.MenuId);
            insertCommand.Parameters.AddWithValue("@name", category.Name);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу dishes
        var dishes = new[]
        {
            new { CategoryId = 1, Price = 200.00, Weight = 150.00, Name = "Салат Цезарь", Description = "Классический салат с курицей", CookingTimeMinutes = 15 },
            new { CategoryId = 2, Price = 500.00, Weight = 300.00, Name = "Стейк", Description = "Сочный говяжий стейк", CookingTimeMinutes = 25 },
            new { CategoryId = 3, Price = 250.00, Weight = 200.00, Name = "Торт Наполеон", Description = "Сладкий десерт с кремом", CookingTimeMinutes = 30 },
            
            new { CategoryId = 1, Price = 180.00, Weight = 120.00, Name = "Салат Оливье", Description = "Традиционный салат с овощами и мясом", CookingTimeMinutes = 10 },
            //new { CategoryId = 2, Price = 400.00, Weight = 350.00, Name = "Куриное филе", Description = "Запеченное куриное филе с травами", CookingTimeMinutes = 20 },
            //new { CategoryId = 3, Price = 150.00, Weight = 100.00, Name = "Пирожное Эклер", Description = "Нежное пирожное с заварным кремом", CookingTimeMinutes = 12 }
        };

        foreach (var dish in dishes)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [dishes] (category_id, price, weight, name, description, cooking_time_minutes) 
                                                  VALUES (@categoryId, @price, @weight, @name, @description, @cookingTimeMinutes);";

            insertCommand.Parameters.AddWithValue("@categoryId", dish.CategoryId);
            insertCommand.Parameters.AddWithValue("@price", dish.Price);
            insertCommand.Parameters.AddWithValue("@weight", dish.Weight);
            insertCommand.Parameters.AddWithValue("@name", dish.Name);
            insertCommand.Parameters.AddWithValue("@description", dish.Description);
            insertCommand.Parameters.AddWithValue("@cookingTimeMinutes", dish.CookingTimeMinutes);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу orders
        var orders = new[]
        {
            new { ClientId = 1, RestaurantId = 1, Comment = "Без перца", Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Status = 0 },
            new { ClientId = 2, RestaurantId = 2, Comment = "Острый", Datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Status = 1 }
        };

        foreach (var order in orders)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [orders] (client_id, restaurant_id, comment, datetime, status) 
                                                  VALUES (@clientId, @restaurantId, @comment, @datetime, @status);";

            insertCommand.Parameters.AddWithValue("@clientId", order.ClientId);
            insertCommand.Parameters.AddWithValue("@restaurantId", order.RestaurantId);
            insertCommand.Parameters.AddWithValue("@comment", order.Comment);
            insertCommand.Parameters.AddWithValue("@datetime", order.Datetime);
            insertCommand.Parameters.AddWithValue("@status", order.Status);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу order_items
        var orderItems = new[]
        {
            new { OrderId = 1, DishId = 1, Count = 2, Price = 200.00 },
            new { OrderId = 1, DishId = 2, Count = 1, Price = 500.00 },
            new { OrderId = 2, DishId = 3, Count = 1, Price = 250.00 }
        };

        foreach (var orderItem in orderItems)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [order_items] (order_id, dish_id, count, price) 
                                                  VALUES (@orderId, @dishId, @count, @price);";

            insertCommand.Parameters.AddWithValue("@orderId", orderItem.OrderId);
            insertCommand.Parameters.AddWithValue("@dishId", orderItem.DishId);
            insertCommand.Parameters.AddWithValue("@count", orderItem.Count);
            insertCommand.Parameters.AddWithValue("@price", orderItem.Price);
            insertCommand.ExecuteNonQuery();
        }

        // Добавление данных в таблицу tables
        var tables = new[]
        {
            new { RestaurantId = 1, Number = 1 },
            new { RestaurantId = 1, Number = 2 },
            new { RestaurantId = 2, Number = 1 }
        };

        foreach (var table in tables)
        {
            using var insertCommand = new SQLiteCommand(connection);
            insertCommand.CommandText = @"INSERT INTO [tables] (restaurant_id, number) 
                                                  VALUES (@restaurantId, @number);";

            insertCommand.Parameters.AddWithValue("@restaurantId", table.RestaurantId);
            insertCommand.Parameters.AddWithValue("@number", table.Number);
            insertCommand.ExecuteNonQuery();
        }
        Console.WriteLine("Данные успешно добавлены во все таблицы.");
    }
    
}