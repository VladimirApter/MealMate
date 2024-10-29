using host.Models;
using Microsoft.EntityFrameworkCore;

namespace host.DataBaseAccess;

public class CategoriesAccess : DataBaseAccess
{
    public static async Task<Category?> GetCategoryAsync(int id)
    {
        await using var context = new ApplicationDbContext();

        var category = await context.Categories.FindAsync(id);
        
        var dishes = await context.Dishes
            .Where(d => d.CategoryId == id)
            .ToListAsync();
        if (category != null)
        {
            category.Dishes = dishes;
        }
        return category;
    }
    
    public static void AddOrUpdateCategory(Category category)
    {
        if (category.Dishes != null)
        {
            foreach (var dish in category.Dishes)
            {
                DishesAccess.AddOrUpdateDish(dish);
            }
        }

        using var context = new ApplicationDbContext();
        context.Categories.Add(category);
        context.SaveChanges();
    }
}