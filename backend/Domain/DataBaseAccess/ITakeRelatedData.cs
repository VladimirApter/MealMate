namespace Domain.DataBaseAccess;

public interface ITakeRelatedData
{
    Task TakeRelatedData(ApplicationDbContext context);
}