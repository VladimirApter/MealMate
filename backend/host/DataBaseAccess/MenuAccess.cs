using host.Models;
namespace host.DataBaseAccess;

public class MenuAccess : DataBaseAccess
{
    private const string MenuQuery = "SELECT id FROM menus WHERE id = @menuId";
    private const string ListCategoryQuery = "SELECT id FROM categories WHERE menu_id = @menuId";

    public static Menu? GetMenu(int id)
    {
        using var menuReader = ExecuteReader(MenuQuery, ("@menuId", id));
        if (!menuReader.Read()) return null;

        var menu = new Menu(id, []);
        using var listCategoryReader = ExecuteReader(ListCategoryQuery, ("@menuId", id));
        while (listCategoryReader.Read())
        {
            var category = CategoriesAccess.GetCategory(listCategoryReader.GetInt32(0));
            menu.Categories.Add(category);
        }

        return menu;
    }
}