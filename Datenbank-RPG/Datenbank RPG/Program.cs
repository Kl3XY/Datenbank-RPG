using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Extensions;
using System.Data.SqlClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
{
    options.TagName = "nav";
    options.TagClasses = "";
    options.OlClasses = "bcList";
    options.LiClasses = "bcItem";
    options.ActiveLiClasses = "bcItem";
    options.SeparatorElement = "";
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

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

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Database}/{action=initServer}/{id?}");

app.Run();
