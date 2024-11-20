namespace Domen.DataBaseAccess;

public interface ITakeRelatedData
{
    Task TakeRelatedData(ApplicationDbContext context);
}