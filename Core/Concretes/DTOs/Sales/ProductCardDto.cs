namespace Core.Concretes.DTOs.Sales
{
    public class ProductCardDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public bool InStock { get; set; }

        public int StoreId { get; set; }
        public string StoreName { get; set; } = null!;

        public int BrandId { get; set; }
        public string BrandName { get; set; } = null!;

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;

        public string? CoverImagePath { get; set; }
    }
}
