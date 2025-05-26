namespace Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; } 
    public bool IsTop { get; set; }
    public bool IsPremium { get; set; }
    public int StockQuantity { get; set; } 
    public DateTime PremiumOrTopExpiryDate { get; set; }

    public virtual Category Category { get; set; }
    public virtual User Users { get; set; }
}