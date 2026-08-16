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

        [HttpPost]

        public async Task<ActionResult<List<DeductStockItemDto>>> DeductItems()
        {
            var items = await _context.Products.ToListAsync();

            foreach (var item in items)
            {
                
            }
        }

    }    


}