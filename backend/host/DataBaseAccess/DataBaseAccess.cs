using System.Collections;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations.Schema;
using host.DataBaseAccess; // Не забудьте подключить этот неймспейс

public class DataBaseAccess<T> where T : class, ITableDataBase
{
    public static async Task<T?> GetAsync(int id)
    {
        await using var context = new ApplicationDbContext();

        var obj = await context.Set<T>().FindAsync(id);

        if (obj is ITakeRelatedData relatedDataLoader)
        {
            await relatedDataLoader.TakeRelatedData(context);
        }

        return obj;
    }

    public static void AddOrUpdate(T obj)
    {
        using var context = new ApplicationDbContext();
        var existingObj = context.Set<T>().Find(obj.Id);

        if (existingObj != null)
            context.Entry(existingObj).CurrentValues.SetValues(obj);

        else
            context.Set<T>().Add(obj);
        AddOrUpdateNotMappedProperties(obj);
        context.SaveChanges();
    }

    private static void AddOrUpdateNotMappedProperties(T obj)
    {
        var type = obj.GetType();
        var properties = type.GetProperties();

        foreach (var property in properties)
        {
            var notMappedAttribute = property.GetCustomAttribute<NotMappedAttribute>();
            if (notMappedAttribute == null) continue;
            var propertyValue = property.GetValue(obj);
            if (propertyValue == null) continue;
            
            InvokeAddOrUpdate(propertyValue);
            
            if (propertyValue is IEnumerable enumerable and not string)
            {
                foreach (var item in enumerable)
                {
                    InvokeAddOrUpdate(item);
                }
            }
        }
    }

    private static void InvokeAddOrUpdate(object item)
    {
        if (item is ITableDataBase)
        {
            var genericMethod = typeof(DataBaseAccess<>)
                .MakeGenericType(item.GetType())
                .GetMethod("AddOrUpdate", [item.GetType()]);

            genericMethod?.Invoke(null, [item]);
        }
    }
}