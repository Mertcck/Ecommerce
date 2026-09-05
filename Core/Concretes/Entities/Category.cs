using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public int? ParentCategoryId { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; } = [];
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
