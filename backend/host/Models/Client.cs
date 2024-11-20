using host.DataBaseAccess;

namespace host.Models;

public class Client : ITableDataBase
{
    public int? Id { get; set; }
    public string Ip { get; set; }
    
    public Client(int? id, string ip)
    {
        Id = id;
        Ip = ip;
    }
}