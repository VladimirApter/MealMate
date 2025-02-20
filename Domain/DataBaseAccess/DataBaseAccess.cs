using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Domain.Logic;
using Domain.Models;
using host;

namespace Domain.DataBaseAccess;

public class DataBaseAccess<T> where T : class, ITableDataBase
{
    public static async Task<T?> GetAsync(long id)
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

        if (obj is Dish or Drink && (obj.Id == null || IsIdDuplicate(context, obj.Id.Value)) &&
            (obj is not Dish || context.Set<Dish>().Find(obj.Id) == null) &&
            (obj is not Drink || context.Set<Drink>().Find(obj.Id) == null))
        {
            obj.Id = GetNextUniqueId(context);
        }

        if (obj is NotificationGetter notificationGetter)
        {
            DataBaseAccess<NotificationGetter>.AddOrUpdateNotificationGetter(notificationGetter);
            context.SaveChanges();
            return;
        }

        var existingObj = context.Set<T>().Find(obj.Id);
        if (existingObj != null)
        {
            RemoveRelatedData(existingObj);

            if (existingObj is OrderItem or Order)
            {
                AddOrUpdateNotMappedProperties(obj);
                object? objNew = null;
                if (obj is OrderItem objOrderItem)
                {
                    objOrderItem.MenuItemId = objOrderItem.MenuItem?.Id;
                    objNew = objOrderItem;
                }
                else if (obj is Order objOrder)
                {
                    objOrder.ClientId = objOrder.Client?.Id;
                    objNew = objOrder;
                }

                if (objNew != null) context.Entry(existingObj).CurrentValues.SetValues(objNew);
            }
            else if (existingObj is Dish or Drink)
            {
                MenuItem? objMenuItem = existingObj is Drink ? obj as Drink : obj as Dish;
                if (objMenuItem != null)
                {
                    if (objMenuItem.NutrientsOf100grams != null)
                        objMenuItem.NutrientsOf100grams.MenuItemId = objMenuItem.Id;
                    object objNew = objMenuItem;
                    AddOrUpdateNotMappedProperties(obj);
                    context.Entry(existingObj).CurrentValues.SetValues(objNew);
                }
            }
            else
            {
                context.Entry(existingObj).CurrentValues.SetValues(obj);
            }
        }
        else
        {
            if (obj is OrderItem or Order)
            {
                AddOrUpdateNotMappedProperties(obj);
                if (obj is OrderItem objOrderItem)
                {
                    objOrderItem.MenuItemId = objOrderItem.MenuItem?.Id;
                    context.Set<OrderItem>().Add(objOrderItem);
                }

                else if (obj is Order objOrder)
                {
                    objOrder.ClientId = objOrder.Client?.Id;
                    context.Set<Order>().Add(objOrder);
                }
            }
            else if (obj is Dish or Drink)
            {
                MenuItem? objMenuItem = obj is Drink ? obj as Drink : obj as Dish;
                if (objMenuItem?.NutrientsOf100grams != null)
                    objMenuItem.NutrientsOf100grams.MenuItemId = objMenuItem.Id;
                AddOrUpdateNotMappedProperties(obj);

                if (objMenuItem is Drink objDrink)
                    context.Set<Drink>().Add(objDrink);
                else if (objMenuItem is Dish objDish)
                    context.Set<Dish>().Add(objDish);
            }
            else
            {
                context.Set<T>().Add(obj);
            }
        }

        AddOrUpdateNotMappedProperties(obj);
        context.SaveChanges();
    }

    public static void Delete(long id)
    {
        using var context = new ApplicationDbContext();

        var obj = context.Set<T>().Find(id);

        if (obj == null) return;

        RemoveRelatedData(obj);
        context.Set<T>().Remove(obj);
        context.SaveChanges();
    }

    public static void AddOrUpdateTable<TTable>(TTable table, bool isNew) where TTable : Table
    {
        using var context = new ApplicationDbContext();
        var existingTable = context.Set<Table>().Find(table.Id);

        if (existingTable != null)
        {
            context.Entry(existingTable).CurrentValues.SetValues(table);
            if (!isNew)
            {
                context.Entry(existingTable).Property(nameof(Table.QRCodeImagePath)).IsModified = false;
                context.Entry(existingTable).Property(nameof(Table.Token)).IsModified = false;
            }
        }
        else if (table.Id == null) context.Set<Table>().Add(table);

        context.SaveChanges();
    }

    private static async void AddOrUpdateNotificationGetter<TNotificationGetter>(TNotificationGetter notificationGetter)
        where TNotificationGetter : NotificationGetter
    {
        await using var context = new ApplicationDbContext();
        var existingNotificationGetter = await context.Set<NotificationGetter>().FindAsync(notificationGetter.Id);

        if (existingNotificationGetter != null)
        {
            context.Entry(existingNotificationGetter).CurrentValues.SetValues(notificationGetter);
        }
        else
        {
            context.Set<NotificationGetter>().Add(notificationGetter);
            await context.SaveChangesAsync();
            await ForwardToPythonServer.ForwardObject(notificationGetter,
                $"{HostsUrlGetter.PyServerUrl}/notificationgetter/");
        }
    }

    private static void RemoveRelatedData(T obj)
    {
        using var context = new ApplicationDbContext();
        if (obj is IDeleteRelatedData relatedDataDelete)
            relatedDataDelete.DeleteRelatedData(context);
    }


    private static bool IsIdDuplicate(ApplicationDbContext context, long id)
    {
        var allIds = context.Dishes.Select(d => d.Id)
            .Union(context.Drinks.Select(d => d.Id));
        return allIds.Contains(id);
    }

    private static long GetNextUniqueId(ApplicationDbContext context)
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

        genericMethod?.Invoke(null, [item]);
    }
}