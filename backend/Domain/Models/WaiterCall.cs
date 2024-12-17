using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Domain.DataBaseAccess;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

public class WaiterCall: ITableDataBase, ITakeRelatedData
{
    public long? Id { get; set; }
    [JsonPropertyName("client_id")] public long? ClientId { get; set; }
    [JsonPropertyName("table_id")] public long TableId { get; set; }
    [JsonPropertyName("date_time")] public DateTime DateTime { get; set; }
    public WaiterCallStatus Status { get; set; }
    [NotMapped] public Client? Client { get; set; }

    public enum WaiterCallStatus
    {
        InAssembly,
        Done
    }

    public async Task TakeRelatedData(ApplicationDbContext context)
    {
        Client = await context.Clients.FirstOrDefaultAsync(c => c.Id == ClientId);
    }
}