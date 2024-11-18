using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.DTOs;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleOrderItemController : ControllerBase
    {
        private readonly DataContext _context;

        public SaleOrderItemController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("GetSaleOrderItemsList")]
        public async Task<ActionResult<IEnumerable<SaleOrderItem>>> GetSaleOrderItemsList()
        {
            return await _context.SaleOrderItems.ToListAsync();
        }

        [HttpGet("GetSaleOrderItem")]
        public async Task<ActionResult<SaleOrderItem>> GetSaleOrderItem(int id)
        {
            var saleOrderItem = await _context.SaleOrderItems.FindAsync(id);
            return saleOrderItem;
        }
        [HttpPut("PutSaleOrderItem")]
        public async Task<IActionResult> PutSaleOrderItem(int id,[FromBody] SaleOrderItemDto saleOrderItemdto)
        {

            var saleOrderItem = await _context.SaleOrderItems.FindAsync(id);
            saleOrderItem.Quantity = saleOrderItemdto.Quantity;
            saleOrderItem.UnitPrice = saleOrderItemdto.UnitPrice;
            saleOrderItem.SaleOrderId = saleOrderItemdto.SaleOrderId;
            saleOrderItem.ProductId = saleOrderItemdto.ProductId;

            _context.Entry(saleOrderItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleOrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Sale Order Item Updated successfully" });
        }

        [HttpPost]
        public async Task<ActionResult<SaleOrderItem>> PostSaleOrderItem([FromBody]SaleOrderItemDto saleOrderItemdto)
        {
            var saleOrderItem = new SaleOrderItem
            {
                Quantity = saleOrderItemdto.Quantity,
                UnitPrice = saleOrderItemdto.UnitPrice,
                SaleOrderId = saleOrderItemdto.SaleOrderId,
                ProductId = saleOrderItemdto.ProductId,
            };
            _context.SaleOrderItems.Add(saleOrderItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sale Order Item Saved successfully" });
        }
        [HttpDelete("DeleteSaleOrderItem")]
        public async Task<IActionResult> DeleteSaleOrderItem(int id)
        {
            var saleOrderItem = await _context.SaleOrderItems.FindAsync(id);
            if (saleOrderItem == null)
            {
                return NotFound();
            }

            _context.SaleOrderItems.Remove(saleOrderItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sale Order Item Deleted successfully" });
        }

        private bool SaleOrderItemExists(int id)
        {
            return _context.SaleOrderItems.Any(e => e.Id == id);
        }
    }
}
