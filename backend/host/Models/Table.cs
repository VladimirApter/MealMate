using System.Text.Json.Serialization;

namespace host.Models;

public class Table
{
    public int Id { get; set; }
    public int Number { get; set; }
    public Table(int number, int id)
    {
        Number = number;
        Id = id;
    }
}