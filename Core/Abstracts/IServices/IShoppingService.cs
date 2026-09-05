using Core.Concretes.DTOs.Sales;
using System.Security.Claims;

namespace Core.Abstracts.IServices
{
    public interface IShoppingService
    {
        public Task<CartDto?> GetCartAsync(ClaimsPrincipal? user);
        public Task<bool> AddToCartAsync(ClaimsPrincipal? user, int productId, int quantity = 1);
        public Task<bool> RemoveFromCartAsync(ClaimsPrincipal? user, int productId);
    }
}
