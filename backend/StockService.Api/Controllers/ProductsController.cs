using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockService.Api.Models;
using StockService.Api.Data;
using StockService.Api.DTOs;

namespace StockService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class ProductsController : ControllerBase
    {
        private readonly StockDbContext _context;

        public ProductsController(StockDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetProductsById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(product);
        }

        [HttpPost]

        public async Task<ActionResult<Product>> PostProducts(Product product)
        {
            if (await _context.Products.AnyAsync(p => p.Code == product.Code))
            {
                return BadRequest("Produto já existente com código digitado");
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductsById), new { id = product.Id }, product);

        }

        [HttpPost("deduct-stock")]
        public async Task<IActionResult> DeductStock([FromBody] List<DeductStockItemDto> items)
        {
            Console.WriteLine($"[StockService] Recebida solicitação de baixa de estoque para {items?.Count ?? 0} itens.");

            if (items == null || !items.Any())
                return BadRequest("Nenhum item informado.");

            foreach (var item in items)
            {
                Console.WriteLine($"[StockService] Validando item - ProductId: {item.ProductId}, Qtd: {item.Quantity}");
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product == null)
                    return BadRequest($"Produto com ID {item.ProductId} não encontrado.");

                if (product.StockQuantity < item.Quantity)
                    return BadRequest($"Saldo insuficiente para o produto '{product.Description}'. Disponível: {product.StockQuantity}, Solicitado: {item.Quantity}.");
            }

            foreach (var item in items)
            {
                var product = await _context.Products.FindAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    Console.WriteLine($"[StockService] Novo saldo para {product.Description}: {product.StockQuantity}");
                }
            }

            await _context.SaveChangesAsync();
            Console.WriteLine("[StockService] Alterações salvas com sucesso no banco!");

            return Ok();
        }

    }    


}