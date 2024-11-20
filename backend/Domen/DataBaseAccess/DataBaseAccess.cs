using System.Collections;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using Domen.DataBaseAccess;
using Domen.Models;

public class DataBaseAccess<T> where T : class, ITableDataBase
{
    public static async Task<T?> GetAsync(int id)
    {
        await using var context = new ApplicationDbContext();

        var obj = await context.Set<T>().FindAsync(id);

        if (obj is ITakeRelatedData relatedDataLoader)
            await relatedDataLoader.TakeRelatedData(context);

        return obj;
    }

    public static void AddOrUpdate(T obj)
    {
        using var context = new ApplicationDbContext();
        
        if (obj is Dish or Drink && (obj.Id == null || IsIdDuplicate(context, obj.Id.Value))) 
            obj.Id = GetNextUniqueId(context);
        
        var existingObj = context.Set<T>().Find(obj.Id);
        if (existingObj != null)
        {
            Remove(obj);
            context.Entry(existingObj).CurrentValues.SetValues(obj);
        }
        else
            context.Set<T>().Add(obj);

        AddOrUpdateNotMappedProperties(obj);
        context.SaveChanges();
    }

    private static void Remove(T obj)
    {
        using var context = new ApplicationDbContext();
        if (obj is IDeleteRelatedData relatedDataDelete) 
            relatedDataDelete.DeleteRelatedData(context);
    }
    

    private static bool IsIdDuplicate(ApplicationDbContext context, int id)
    {
        var allIds = context.Dishes.Select(d => d.Id)
            .Union(context.Drinks.Select(d => d.Id));
        return allIds.Contains(id);
    }

    private static int GetNextUniqueId(ApplicationDbContext context)
    {
        var maxDishId = context.Dishes.Any() ? context.Dishes.Max(d => d.Id) : 0;
        var maxDrinkId = context.Drinks.Any() ? context.Drinks.Max(d => d.Id) : 0;

        maxDishId ??= 0;
        maxDrinkId ??= 0;
        
        return Math.Max(maxDishId.Value, maxDrinkId.Value) + 1;
    }

    private static void AddOrUpdateNotMappedProperties(T obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            if (property.GetCustomAttribute<NotMappedAttribute>() == null) continue;

            var propertyValue = property.GetValue(obj);
            if (propertyValue == null) continue;

            ProcessValue(propertyValue);
        }
    }

    private static void ProcessValue(object propertyValue)
    {
        if (propertyValue is ITableDataBase tableData)
            InvokeAddOrUpdate(tableData);
        else if (propertyValue is IEnumerable enumerable and not string)
            foreach (var item in enumerable)
                if (item is ITableDataBase tableDataItem) 
                    InvokeAddOrUpdate(tableDataItem);
    }

    private static void InvokeAddOrUpdate(ITableDataBase item)
    {
        var genericMethod = typeof(DataBaseAccess<>)
            .MakeGenericType(item.GetType())
            .GetMethod(nameof(AddOrUpdate), BindingFlags.Public | BindingFlags.Static);

        genericMethod?.Invoke(null, new object[] { item });
    }
}