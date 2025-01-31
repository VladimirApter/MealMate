namespace Domain.Models;

public class TgAccount
{
    public long? Id { get; set; }
    public string Username { get; set; }

    protected TgAccount(){}
    protected TgAccount(long? id, string username)
    {
        Id = id;
        Username = username;
    }
}