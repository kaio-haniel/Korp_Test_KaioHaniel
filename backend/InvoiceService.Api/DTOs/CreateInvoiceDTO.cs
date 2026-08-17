using System.ComponentModel.DataAnnotations;

namespace InvoiceService.Api.DTOs;

public class CreateInvoiceItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Required]
    public string ProductCode { get; set; } = string.Empty;

    [Required]
    public string ProductDescription { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser de no mínimo 1.")]
    public int Quantity { get; set; }
}

public class CreateInvoiceDto
{
    [Required(ErrorMessage = "A nota deve conter pelo menos um item.")]
    [MinLength(1, ErrorMessage = "A nota deve conter pelo menos um item.")]
    public List<CreateInvoiceItemDto> Items { get; set; } = new();
}