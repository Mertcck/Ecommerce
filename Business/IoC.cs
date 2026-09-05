using Business.Services;
using Core.Abstracts.IServices;
using Core.Concretes.Entities;
using Core.Utils;
using Data;
using Data.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class IoC
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ShopContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("shop")));

            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ShopContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(cfg => { cfg.AddProfile<MapProfiles>(); });

            // Servisler
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IStoreProductService, StoreProductService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<IHomeService, HomeService>();
            services.AddScoped<IShoppingService, ShoppingService>();

            return services;
        }
    }
}
