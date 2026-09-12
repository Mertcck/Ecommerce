using Business;
using Core.Abstracts.IServices;
using System.Security.Claims;

internal class Program
{
    private static void Main(string[] args)
    {
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

            var cart = await service.GetCartAsync(User);
            
            return new // Anonim sınıf olarak cevap dönüldü.
            {
                Result = result,
                Message = result ? "Ürün sepete eklendi." : "Ürün sepete eklenemedi!",
                CurrentCart = cart
            }; 
        });

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }
}