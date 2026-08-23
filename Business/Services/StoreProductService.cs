using AutoMapper;
using Core.Abstracts.IServices;
using Core.Concretes.DTOs.Product;
using Core.Concretes.Entities;
using Core.Concretes.Enums;
using Core.Utils;
using Microsoft.AspNetCore.Identity;

namespace Business.Services
{
    public class StoreProductService : IStoreProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public StoreProductService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        public async Task<bool> AddProductFeatureAsync(int storeId, int productId, CreateProductFeatureDto model)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            var featureRepo = unitOfWork.Repository<ProductFeature>();
            var feature = mapper.Map<ProductFeature>(model);
            feature.ProductId = productId;
            feature.DisplayOrder = 999;
            await featureRepo.CreateOneAsync(feature);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> AddProductImageAsync(int storeId, int productId, CreateProductImageDto model, string wwwroot)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            if (model.File == null || model.File.Length == 0) return false;

            string uploadFolder = Path.Combine(wwwroot, "uploads", $"store_{storeId}");
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;
            string filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(fileStream);
            }
            string imageUrl = $"/uploads/store_{storeId}/{uniqueFileName}";

            var imageRepo = unitOfWork.Repository<ProductImage>();
            var image = new ProductImage
            {
                ProductId = productId,
                ImageUrl = imageUrl,
                DisplayOrder = 999
            };

            await imageRepo.CreateOneAsync(image);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto, int storeId)
        {
            var product = mapper.Map<Product>(createProductDto);
            product.StoreId = storeId;
            var repo = unitOfWork.Repository<Product>();
            await repo.CreateOneAsync(product);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteProductAsync(int productId, int storeId)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = await repo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;
            repo.DeleteOne(productId);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteProductFeatureAsync(int storeId, int productId, int featureId)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            var featureRepo = unitOfWork.Repository<ProductFeature>();
            var feature = await featureRepo.ReadByIdAsync(featureId);
            if (feature == null || feature.ProductId != productId) return false;

            featureRepo.DeleteOne(feature);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> DeleteProductImageAsync(int storeId, int productId, int imageId)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            var imageRepo = unitOfWork.Repository<ProductImage>();
            var image = await imageRepo.ReadByIdAsync(imageId);
            if (image == null || image.ProductId != productId) return false;

            imageRepo.DeleteOne(image);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<int> GetCurrentStoreIdAsync(string user_id)
        {
            var user = await userManager.FindByIdAsync(user_id);
            if (user != null && user.UserType == UserType.StoreOwner)
            {
                var repo = unitOfWork.Repository<Store>();
                if (await repo.AnyAsync(x => x.AppUserId == user_id))
                {
                    return user.StoreProfile!.Id;
                }
            }
            return 0;
        }

        public async Task<IEnumerable<ProductFeatureDto>> GetProductFeaturesAsync(int storeId, int productId)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return [];

            var featureRepo = unitOfWork.Repository<ProductFeature>();
            var features = await featureRepo.ReadManyAsync(x => x.ProductId == productId);
            return mapper.Map<IEnumerable<ProductFeatureDto>>(features.OrderBy(x => x.DisplayOrder));
        }

        public async Task<IEnumerable<ProductImageDto>> GetProductImagesAsync(int storeId, int productId)
        {
            var productRepo = unitOfWork.Repository<Product>();
            var product = await productRepo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return [];

            var imageRepo = unitOfWork.Repository<ProductImage>();
            var images = await imageRepo.ReadManyAsync(x => x.ProductId == productId);
            return mapper.Map<IEnumerable<ProductImageDto>>(images.OrderBy(x => x.DisplayOrder));
        }

        public async Task<StoreProductDto?> GetStoreProductAsync(int productId)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = await repo.ReadByIdAsync(productId);
            return product != null ? mapper.Map<StoreProductDto>(product) : null;
        }

        public async Task<UpdateProductDto?> GetStoreProductForEditAsync(int productId, int storeId)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = await repo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return null;

            return mapper.Map<UpdateProductDto>(product);
        }

        public async Task<IEnumerable<StoreProductListDto>> GetStoreProductsAsync(int storeId)
        {
            var repo = unitOfWork.Repository<Product>();
            var products = await repo.ReadManyAsync(x => x.StoreId == storeId);
            return mapper.Map<IEnumerable<StoreProductListDto>>(products);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, int storeId)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = mapper.Map<Product>(updateProductDto);
            product.StoreId = storeId;
            repo.UpdateOne(product);
            int rows = await unitOfWork.CommitAsync();
            return rows > 0;
        }

        public async Task<bool> UpdateProductFeatureDisplayOrderAsync(int storeId, int productId, Dictionary<int, int> featureOrders)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = await repo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            var featureRepo = unitOfWork.Repository<ProductFeature>();
            var features = await featureRepo.ReadManyAsync(f => f.ProductId == productId);
            bool isUpdated = false;

            foreach (var feature in features)
            {
                if (featureOrders.TryGetValue(feature.Id, out int newOrder))
                {
                    feature.DisplayOrder = newOrder;
                    featureRepo.UpdateOne(feature);
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                int rows = await unitOfWork.CommitAsync();
                return rows > 0;
            }

            return false;
        }

        public async Task<bool> UpdateProductImageDisplayOrderAsync(int storeId, int productId, Dictionary<int, int> imageOrders)
        {
            var repo = unitOfWork.Repository<Product>();
            var product = await repo.ReadByIdAsync(productId);
            if (product == null || product.StoreId != storeId) return false;

            var imageRepo = unitOfWork.Repository<ProductImage>();
            var images = await imageRepo.ReadManyAsync(f => f.ProductId == productId);
            bool isUpdated = false;

            foreach (var img in images)
            {
                if (imageOrders.TryGetValue(img.Id, out int newOrder))
                {
                    img.DisplayOrder = newOrder;
                    imageRepo.UpdateOne(img);
                    isUpdated = true;
                }
            }

            if (isUpdated)
            {
                int rows = await unitOfWork.CommitAsync();
                return rows > 0;
            }

            return false;
        }
    }
}
