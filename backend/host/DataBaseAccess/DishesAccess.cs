using System.Data.SQLite;
using host.Models;
using Newtonsoft.Json.Serialization;
using System.Text.Json.Serialization;
namespace host.DataBaseAccess;

public class DishesAccess : DataBaseAccess
{
    public static async Task<Dish?> GetDishAsync(int id)
    {
        await using var context = new ApplicationDbContext();
        var dish = await context.Dishes.FindAsync(id);
        return dish;
    }


    public static void AddOrUpdateDish(Dish dish)
    {
        using var context = new ApplicationDbContext();

        // Проверяем, существует ли блюдо
        var existingDish = context.Dishes.Find(dish.Id);

        if (existingDish != null)
        {
            // Если блюдо найдено, обновляем его свойства
            context.Entry(existingDish).CurrentValues.SetValues(dish);
        }
        else
        {
            // Если блюдо не найдено, добавляем новое
            context.Dishes.Add(dish);
        }

        // Сохраняем изменения
        context.SaveChanges();
    }
}