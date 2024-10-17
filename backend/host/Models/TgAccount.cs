namespace host.Models;

public class TgAccount
{
    public int? Id { get; set; }
    public string Username { get; set; }

    protected TgAccount(string username, int? id)
    {
        Username = username;
        Id = id;
    }
}