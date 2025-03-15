using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FinancialWebApplication.Data;
using FinancialWebApplication.Models;
using Microsoft.AspNetCore.Identity;

// for authenication based using cookies
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FinancialWebApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FinancialWebApplicationContext") ?? throw new InvalidOperationException("Connection string 'FinancialWebApplicationContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
        options.SlidingExpiration = true;
        // Create a Forbidden View for error page
        options.AccessDeniedPath = "/Forbidden";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
    