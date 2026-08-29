using Core.Concretes.DTOs.Sales;

namespace Core.Abstracts.IServices
{
    public interface IHomeService
    {
        Task<IEnumerable<ProductCardDto>> GetAllAsync();
    }
}
