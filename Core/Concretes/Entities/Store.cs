using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Store : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public virtual AppUser AppUser { get; set; } = null!;
        public string StoreName { get; set; } = null!;
        public string Slug { get; set; } = string.Empty;
        public string TaxOffice { get; set; } = null!;
        public string TaxNumber { get; set; } = null!;
        public string ContactPhone { get; set; } = null!;
        public string ContactEmail { get; set; } = null!;
        public decimal CommissionRate { get; set; }
        public bool Verified { get; set; } = false;
        public virtual ICollection<Product> Products { get; set; } = [];
    }
}
