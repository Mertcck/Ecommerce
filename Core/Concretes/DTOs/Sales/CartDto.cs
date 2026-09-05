namespace Core.Concretes.DTOs.Sales
{
    public class CartDto
    {
        public int Id { get; set; }
        public IEnumerable<CartItemDto> Items { get; set; } = [];
        public decimal CartTotal => Items.Any() ? Items.Sum(item => item.TotalPrice) : 0;
        public int ItemCount => Items.Count();
    }
}
