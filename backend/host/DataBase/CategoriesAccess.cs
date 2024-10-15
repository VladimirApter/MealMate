using host.Models;
namespace host.DataBase;

public class CategoriesAccess : DataBaseAccess
{
    private const string CategoryQuery = "SELECT name FROM categories WHERE id = @categoryId";
    private const string ListDishQuery = "SELECT id FROM dishes WHERE category_id = @categoryId";
    public static Category? GetCategory(int id)
    {
        using var categoryReader = ExecuteReader(CategoryQuery, ("@categoryId", id));
        if (!categoryReader.Read()) return null;
        var category = new Category(categoryReader.GetString(0), []);
        
        using var listDishReader = ExecuteReader(ListDishQuery, ("@categoryId", id));
        while (listDishReader.Read())
        {
            var dish = DishesAccess.GetDish(listDishReader.GetInt32(0));
            category.Dishes.Add(dish);
        }

        return category;
    }
}