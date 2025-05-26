using Domain.Enums;

namespace Domain.Filters;

public class OrderFilter
{
    public int? UserId { get; set; }
    public int? ProductId { get; set; }
    public Status? Status { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }  
}