namespace Furbar.Models.Sales;

public class Sale
{
    public int Id { get; set; }
    public double TotalPrice { get; set; }

    public string? AppUserId { get; set; }
    public DateTime? CreatedDate { get; set; }
    public List<SalesProducts>? SalesProducts { get; set; }
}
