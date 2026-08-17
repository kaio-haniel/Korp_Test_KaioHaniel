using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InvoiceService.Api.Models;

public class InvoiceItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int InvoiceId { get; set; }

    [Required]
    public int ProductId { get; set; }

    [Required]
    [MaxLength(50)]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string ProductDescription { get; set; } = string.Empty;

    [Required]
    public int Quantity { get; set; }

    [JsonIgnore]

    public Invoice? Invoice { get; set; }

}