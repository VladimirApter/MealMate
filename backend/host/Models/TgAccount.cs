namespace host.Models;

public class TgAccount
{
    public int? Id { get; set; }
    public string Username { get; set; }

    protected TgAccount(int? id, string username)
    {
        Id = id;
        Username = username;
    }
}