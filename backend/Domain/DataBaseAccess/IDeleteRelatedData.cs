namespace Domain.DataBaseAccess;

public interface IDeleteRelatedData
{
    void DeleteRelatedData(ApplicationDbContext context);
}