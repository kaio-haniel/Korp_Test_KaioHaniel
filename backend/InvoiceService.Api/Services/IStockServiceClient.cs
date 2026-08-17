namespace InvoiceService.Api.Services;

public interface IStockServiceClient
{
    Task<(bool Success, string? ErrorMessage)> DeductStockAsync(List<DeductStockItemDto> items);
}

public class DeductStockItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}