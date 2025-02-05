namespace Furbar.ViewModels
{
    public class CartVM
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public double Price { get; set; }
        public string? ImageUrl { get; set; }
        public int BasketCount { get; set; }
    }
}
