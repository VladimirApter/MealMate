using System.Text.Json.Serialization;

namespace host.Models;

public class Table
{
    public int Number { get; set; }
    public Table(int number)
    {
        Number = number;
    }
}