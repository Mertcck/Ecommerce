using AutoMapper;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs.Sales;
using Core.Concretes.Entities;
using Core.Concretes.Enums;
using Core.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Business.Services
{
    public class ShoppingService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager) : IShoppingService
    {
        public async Task<bool> AddToCartAsync(ClaimsPrincipal? user, int productId, int quantity = 1)
        {
            var cart = await GetCartAsync(user);
            if (cart != null)
            {
                var repo = unitOfWork.Repository<CartItem>();
                var item = await repo.Query().Where(ci => ci.ProductId == productId && ci.CartId == cart.Id).FirstOrDefaultAsync();
                if (item == null)
                {
                    item = new CartItem
                    {
                        ProductId = productId,
                        CartId = cart.Id,
                        Quantity = quantity
                    };
                    await repo.CreateOneAsync(item);
                }
                else
                {
                    item.Quantity += quantity;
                    repo.UpdateOne(item);
                }
                await unitOfWork.CommitAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDto?> GetCartAsync(ClaimsPrincipal? user)
        {
            if (user != null)
            {
                var appUser = await userManager.GetUserAsync(user);
                if (appUser != null && appUser.UserType == UserType.Customer)
                {
                    var repo = unitOfWork.Repository<Cart>();
                    var cart = await repo.Query()
                                         .OrderBy(c => c.CreatedAt)
                                         .Where(c => c.CustomerId == appUser.CustomerProfile!.Id && c.Active)
                                         .FirstOrDefaultAsync();
                    if (cart == null)
                    {
                        cart = new Cart { CustomerId = appUser.CustomerProfile!.Id };
                        await repo.CreateOneAsync(cart);
                        await unitOfWork.CommitAsync();
                    }
                    return mapper.Map<CartDto>(cart);
                }
            }
            return null;
        }

        public async Task<bool> RemoveFromCartAsync(ClaimsPrincipal? user, int productId)
        {
            var cart = await GetCartAsync(user);
            if (cart != null)
            {
                var repo = unitOfWork.Repository<CartItem>();
                var item = await repo.Query().Where(ci => ci.ProductId == productId && ci.CartId == cart.Id).FirstOrDefaultAsync();
                if (item != null)
                {
                    repo.DeleteOne(item);
                    await unitOfWork.CommitAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
