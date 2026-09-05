namespace Core.Concretes.DTOs.Sales
{
    public class CartDto
    {
        public int Id { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; } = [];
        public decimal CartTotal => Items.Sum(item => item.TotalPrice);
        public int ItemCount => Items.Count();
    }
}
