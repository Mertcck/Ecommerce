using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = string.Empty;
        public string? LogoUrl { get; set; }
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
