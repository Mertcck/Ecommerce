using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class Customer : BaseEntity
    {
        public string AppUserId { get; set; } = null!;
        public virtual AppUser AppUser { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public virtual ICollection<Cart> Carts { get; set; } = [];
    }
}
