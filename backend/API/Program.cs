using Domain.DataBaseAccess;
using Domain.Logic;

var builder = WebApplication.CreateBuilder(args);


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