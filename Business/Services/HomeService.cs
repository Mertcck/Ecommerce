using AutoMapper;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs.Sales;
using Core.Concretes.Entities;
using Core.Utils;

namespace Business.Services
{
    public class HomeService(IUnitOfWork unitOfWork, IMapper mapper) : IHomeService
    {
        public async Task<IEnumerable<ProductCardDto>> GetAllAsync()
        {
            var repo = unitOfWork.Repository<Product>();
            var products = await repo.ReadManyAsync(x => x.Active);
            return mapper.Map<IEnumerable<ProductCardDto>>(products);
        }
    }
}
