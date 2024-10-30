using host.Models;

namespace host.DataBaseAccess;

public class CategoriesAccess : DataBaseAccess
{
    private const string CategoryQuery = "SELECT name, id, menu_id FROM categories WHERE id = @categoryId";
    private const string ListDishQuery = "SELECT id FROM dishes WHERE category_id = @categoryId";

    private const string InsertCommand =
        @"INSERT INTO [categories] (menu_id, name) 
             VALUES (@menuId, @name);";

    private const string UpdateCommand = @"UPDATE [categories] 
             SET menu_id = @menuId, name = @name 
             WHERE Id = @id;";

    public static Category? GetCategory(int id)
    {
        using var categoryReader = ExecuteReader(CategoryQuery, ("@categoryId", id));
        if (!categoryReader.Read()) return null;
        var category = new Category(categoryReader.GetString(0), categoryReader.GetInt32(1), categoryReader.GetInt32(2),
            []);

        using var listDishReader = ExecuteReader(ListDishQuery, ("@categoryId", id));
        while (listDishReader.Read())
        {
            var dish = DishesAccess.GetDish(listDishReader.GetInt32(0));
            if (dish != null) category.Dishes?.Add(dish);
        }

        return category;
    }

    public static void AddOrUpdateCategory(Category category)
    {
        if (category.Dishes != null)
        {
            foreach (var dish in category.Dishes)
            {
                if (dish is Dish dish1)
                {
                    DishesAccess.AddOrUpdateDish(dish1);
                }
            }
        }

        AddOrUpdateObject(category, InsertCommand, UpdateCommand, (categoryObj, insertOrUpdateCommand) =>
        {
            insertOrUpdateCommand.Parameters.AddWithValue("@menuId", categoryObj.MenuId);
            insertOrUpdateCommand.Parameters.AddWithValue("@name", categoryObj.Name);
        });
    }
}