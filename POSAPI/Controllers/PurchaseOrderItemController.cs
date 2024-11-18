using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.DTOs;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderItemController : ControllerBase
    {
        private readonly DataContext _context;

        public PurchaseOrderItemController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("GetPurchaseOrderItemsList")]
        public async Task<ActionResult<IEnumerable<PurchaseOrderItem>>> GetPurchaseOrderItemsList()
        {
            return await _context.PurchaseOrderItems.ToListAsync();
        }
        [HttpGet("GetPurchaseOrderItem")]
        public async Task<ActionResult<PurchaseOrderItem>> GetPurchaseOrderItem(int id)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            return Ok(purchaseOrderItem);
        }
        [HttpPut("PutPurchaseOrderItem")]
        public async Task<IActionResult> PutPurchaseOrderItem(int id, [FromBody] PurchaseOrderItemDto purchaseOrderItemDto)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            purchaseOrderItem.Quantity = purchaseOrderItemDto.Quantity;
            purchaseOrderItem.UnitPrice = purchaseOrderItemDto.UnitPrice;
            purchaseOrderItem.PurchaseOrderId = purchaseOrderItemDto.PurchaseOrderId;
            purchaseOrderItem.ProductId = purchaseOrderItemDto.ProductId;
            _context.Entry(purchaseOrderItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(new { message = "Purchase Order Item Updated successfully" });
        }

        [HttpPost("PostPurchaseOrderItem")]
        public async Task<ActionResult<PurchaseOrderItem>> PostPurchaseOrderItem([FromBody] PurchaseOrderItemDto purchaseOrderItemDto)
        {
            var purchaseOrderItem = new PurchaseOrderItem
            {
                Quantity = purchaseOrderItemDto.Quantity,
                UnitPrice = purchaseOrderItemDto.UnitPrice,
                PurchaseOrderId = purchaseOrderItemDto.PurchaseOrderId,
                ProductId = purchaseOrderItemDto.ProductId,
            };
            _context.PurchaseOrderItems.Add(purchaseOrderItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Purchase Order Item Saved successfully", data = purchaseOrderItem });
        }

        [HttpDelete("DeletePurchaseOrderItem")]
        public async Task<IActionResult> DeletePurchaseOrderItem(int id)
        {
            var purchaseOrderItem = await _context.PurchaseOrderItems.FindAsync(id);
            _context.PurchaseOrderItems.Remove(purchaseOrderItem);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Purchase Order Item Deleted successfully" });
        }

        private bool PurchaseOrderItemExists(int id)
        {
            return _context.PurchaseOrderItems.Any(e => e.Id == id);
        }
    }
}
