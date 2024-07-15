using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using puntodeventa.Data;
using puntodeventa.Services;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers(); 
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddScoped<ProductosService>();
builder.Services.AddScoped<VentasService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("*") // Reemplaza con los orígenes permitidos
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddBlazorise(options =>
{
    options.Immediate = true;
}).AddBootstrap5Components().AddBootstrap5Providers();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllers(); // Mapear los controladores
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
