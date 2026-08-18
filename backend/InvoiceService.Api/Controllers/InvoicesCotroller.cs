using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceService.Api.Models;
using InvoiceService.Api.Data;
using InvoiceService.Api.DTOs;
using InvoiceService.Api.Services;
using Microsoft.AspNetCore.Http.Features;

namespace InvoiceService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class InvoicesController : ControllerBase
    {
        private readonly InvoiceDbContext _context;

        private readonly IStockServiceClient _stockClient;

        public InvoicesController(InvoiceDbContext context, IStockServiceClient stockClient)
        {
            _context = context;
            _stockClient = stockClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.Include(i => i.Items).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetInvoiceById(int id)
        {

            var invoice = await _context.Invoices
            .Include(i => i.Items)
            .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound("Produto não encontrado.");
            }
            return Ok(invoice);

        }

        [HttpPost]
        public async Task<ActionResult<Invoice>> PostInvoice(CreateInvoiceDto dto)
        {
            var lastnumber = await _context.Invoices.MaxAsync(i => (int?)i.Number) ?? 0;
            var nextnumber = lastnumber + 1;

            Invoice invoice = new();
            invoice.Number = nextnumber;
            invoice.Status = InvoiceStatus.Open;
            invoice.createAt = DateTime.UtcNow;

            invoice.Items = dto.Items.Select(item => new InvoiceItem
            {
                ProductId = item.ProductId,
                ProductCode = item.ProductCode,
                ProductDescription = item.ProductDescription,
                Quantity = item.Quantity
            }).ToList();

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoiceById), new { id = invoice.Id }, invoice);
        }

        [HttpPost("{id}/close")]
        public async Task<ActionResult<Invoice>> CloseInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound("Nota fiscal não encontrada.");
            }

            if (invoice.Status == InvoiceStatus.Closed)
            {
                return BadRequest("Esta nota fiscal já foi fechada ou impressa.");
            }

            var stockItems = invoice.Items.Select(item => new DeductStockItemDto
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity
            }).ToList();

            var (success, errorMessage) = await _stockClient.DeductStockAsync(stockItems);

            if (!success)
            {
                return BadRequest(errorMessage);
            }

            invoice.Status = InvoiceStatus.Closed;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Nota fiscal fechada e impressa com sucesso!", invoice });
        }
        

    }
    
}