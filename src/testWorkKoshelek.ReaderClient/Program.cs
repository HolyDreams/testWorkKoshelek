var builder = WebApplication.CreateBuilder(args);

var congig = builder.Configuration;

var services = builder.Services;
// Add services to the container.
services.Configure<Core.Domain.Models.Settings.WebSocketOptions>(congig.GetSection("WebSocketOptions"));
services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
