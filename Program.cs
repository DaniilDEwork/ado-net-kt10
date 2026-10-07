using Kt10Validation.Data;
using Kt10Validation.Models;
using Kt10Validation.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Kt10Users"));
builder.Services.AddSingleton<UserWriteLock>();
builder.Services.AddScoped<PasswordHasher<AppUser>>();
builder.Services.AddScoped<UserService>();
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseExceptionHandler("/Home/Error");
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
