using AutoMapper;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs.Sales;
using Core.Concretes.Entities;
using Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace Business.Services
{
    public class HomeService(IUnitOfWork unitOfWork, IMapper mapper) : IHomeService
    {
        public async Task<IEnumerable<ProductCardDto>> GetAllAsync(ProductsFilterDto filter)
        {
            var repo = unitOfWork.Repository<Product>();
            var query = repo.Query().Where(x => x.Active);

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(filter.StoreName))
            {
                var storeUrlName = filter.StoreName.ToUrl();
                query = query.Where(p => p.Store.StoreName.ToUrl().Equals(storeUrlName));
            }

            if (!string.IsNullOrWhiteSpace(filter.CategoryName))
            {
                var categoryUrlName = filter.CategoryName.ToUrl();
                query = query.Where(p => p.Category.Name.ToUrl().Equals(categoryUrlName));
            }

            if (!string.IsNullOrWhiteSpace(filter.BrandName))
            {
                var brandUrlName = filter.BrandName.ToUrl();
                query = query.Where(p => p.Brand.Name.ToUrl().Equals(brandUrlName));
            }

            var products = await query.ToListAsync();
            return mapper.Map<IEnumerable<ProductCardDto>>(products);
        }
    }
}
