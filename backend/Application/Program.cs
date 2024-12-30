using System.Net;

var builder = WebApplication.CreateBuilder(args);

var certPath = Path.Combine("certs", "certificate.pfx");

builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    serverOptions.Listen(IPAddress.Any, 5011);
    serverOptions.Listen(IPAddress.Any, 5002, listenOptions =>
    {
        listenOptions.UseHttps(certPath, "mealmate14");
    });
});
builder.Services.AddControllersWithViews();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API V1");
    c.RoutePrefix = "api_docs";
});


app.MapControllers();


app.Run();