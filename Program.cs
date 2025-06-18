using QRCodeBooking.BAL_DAL;
using QRCodeBooking.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using QRCodeBooking.Models.DB;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    })
    .AddRazorRuntimeCompilation();

builder.Services.AddRazorPages();

// Database connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")),
    ServiceLifetime.Transient);

// CORS
builder.Services.AddCors();

// Session configuration
double sessionTimeout = Convert.ToDouble(builder.Configuration["sessionTimeOut"]);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(sessionTimeout);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Misc services
builder.Services.AddHttpContextAccessor();
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();

 
// ✅ Manual Order System Services
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Optional: Custom view path for shared layout
builder.Services.AddControllersWithViews()
    .AddRazorOptions(options =>
    {
        options.ViewLocationFormats.Add("/Views/Shared/_Layout.cshtml");
    });

var app = builder.Build();

// Error page for production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Static files
app.UseStaticFiles();

// Routing and CORS
app.UseRouting();
app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

 
app.UseSession();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Order}/{action=Dashboard}/{id?}");

app.Run();
