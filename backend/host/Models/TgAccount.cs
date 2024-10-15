namespace host.Models;

public class TgAccount
{
    public string Username { get; set; }

    protected TgAccount(string username)
    {
        Username = username;
    }
}