using host.Models;

namespace host.DataBaseAccess;

public class MenuAccess : DataBaseAccess
{
    private const string MenuQuery = "SELECT id, restaurant_id FROM menus WHERE id = @menuId";
    private const string ListCategoryQuery = "SELECT id FROM categories WHERE menu_id = @menuId";

    private const string InsertCommand =
        @"INSERT INTO [menus] (restaurant_id) 
             VALUES (@restaurantId);";

    private const string UpdateCommand = @"UPDATE [menus] 
             SET restaurant_id = @restaurantId 
             WHERE Id = @id;";

    public static Menu? GetMenu(int id)
    {
        using var menuReader = ExecuteReader(MenuQuery, ("@menuId", id));
        if (!menuReader.Read()) return null;

        var menu = new Menu(menuReader.GetInt32(0), [], menuReader.GetInt32(1));
        using var listCategoryReader = ExecuteReader(ListCategoryQuery, ("@menuId", id));
        while (listCategoryReader.Read())
        {
            var category = CategoriesAccess.GetCategoryAsync(listCategoryReader.GetInt32(0)).Result;
            if (category != null) menu.Categories?.Add(category);
        }

        return menu;
    }

    public static void AddOrUpdateMenu(Menu menu)
    {
        if (menu.Categories != null)
        {
            foreach (var category in menu.Categories)
            {
                CategoriesAccess.AddOrUpdateCategory(category);
            }
        }

        AddOrUpdateObject(menu, InsertCommand, UpdateCommand,
            (menuObj, insertOrUpdateCommand) =>
            {
                insertOrUpdateCommand.Parameters.AddWithValue("@restaurantId", menuObj.RestaurantId);
            });
    }
}