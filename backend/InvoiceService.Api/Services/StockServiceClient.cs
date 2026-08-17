using System.Net.Http.Json;

namespace InvoiceService.Api.Services;

public class StockServiceClient : IStockServiceClient
{
    private readonly HttpClient _httpClient;

    public StockServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(bool Success, string? ErrorMessage)> DeductStockAsync(List<DeductStockItemDto> items)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/products/deduct-stock", items);

            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return (false, string.IsNullOrWhiteSpace(errorContent) ? "Falha ao atualizar estoque." : errorContent);
        }
        catch (HttpRequestException ex)
        {
            return (false, $"Não foi possível conectar ao serviço de estoque: {ex.Message}");
        }
        catch (Exception ex)
        {
            return (false, $"Erro inesperado na comunicação com o estoque: {ex.Message}");
        }
    }
}