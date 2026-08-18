using System.ComponentModel.DataAnnotations;

namespace InvoiceService.Api.Models;

public class Invoice
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int Number { get; set; }

    [Required]
    public InvoiceStatus Status { get; set; } = InvoiceStatus.Open;

    public DateTime createAt { get; set; } = DateTime.UtcNow;

    public List<InvoiceItem> Items { get; set; } = new ();

}