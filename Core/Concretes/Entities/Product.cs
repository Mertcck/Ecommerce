using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string SKU { get; set; } = null!;
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; } = null!;
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; } = null!;
        public int StoreId { get; set; }
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<ProductImage> Images { get; set; } = [];
        public virtual ICollection<ProductFeature> Features { get; set; } = [];
        public virtual ICollection<CartItem> CartItems { get; set; } = [];
    }
}
