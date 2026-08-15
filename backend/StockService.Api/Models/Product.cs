using System.ComponentModel.DataAnnotations;

namespace StockService.Api.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Description { get; set; } = string.Empty;

    [Required]
    public int StockQuantity { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; }

    
}