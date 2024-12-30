using System.Net;
using Domain.DataBaseAccess;
using Domain.Logic;


var builder = WebApplication.CreateBuilder(args);

var env = builder.Environment;
var certPath = Path.Combine("..", "certs", "certificate.pfx");
Console.WriteLine(certPath);
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, 5051); 
    serverOptions.Listen(IPAddress.Any, 5001, listenOptions =>
    {
        listenOptions.UseHttps(certPath, "mealmate14");
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseRouting();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = "api_docs";
});

app.MapControllers();


var databaseFolder = DataBasePathGetter.DataBasePath;
if (!Directory.Exists(databaseFolder))
    Directory.CreateDirectory(databaseFolder);

using var context = new ApplicationDbContext();

context.Database.EnsureCreated();


app.Run();