using Business;
using Business.Services;
using Core.Abstracts.IServices;
using Core.Concretes.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddBusiness(builder.Configuration);

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

app.MapPost("/api/cart/add/{id}/{qty?}", async (IShoppingService service, ClaimsPrincipal User, int id, int qty = 1) =>
{
    bool result = await service.AddToCartAsync(User, id, qty);
    return new { Result = result, Message = result ? "Ürün sepete eklendi." : "Ürün sepete eklenemedi!" }; // Anonim sınıf olarak cevap dönüldü.
});

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
