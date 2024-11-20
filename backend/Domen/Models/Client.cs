namespace Domen.Models;

public class Client
{
    public int? Id { get; set; }
    public string Ip { get; set; }
    
    public Client(int? id, string ip)
    {
        Id = id;
        Ip = ip;
    }
}