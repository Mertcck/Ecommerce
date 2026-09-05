using Core.Abstracts.Bases;

namespace Core.Concretes.Entities
{
    public class CartItem : BaseEntity
    {
        public int CartId { get; set; }
        public virtual Cart Cart { get; set; } = null!;
        public int ProductId { get; set; }
        public virtual Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
