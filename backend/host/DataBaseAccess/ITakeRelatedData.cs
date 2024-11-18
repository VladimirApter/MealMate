namespace host.DataBaseAccess;

public interface ITakeRelatedData
{
    Task TakeRelatedData(ApplicationDbContext context);
}