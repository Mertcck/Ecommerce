namespace Core.Concretes.DTOs.Sales
{
    public class ProductsFilterDto
    {
        public string? SearchTerm { get; set; }
        public string? StoreName { get; set; }
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
    }
}
