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
    public class PurchaseOrderController : ControllerBase
    {
        private readonly DataContext _context;

        public PurchaseOrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetPurchaseOrdersList")]
        public async Task<ActionResult<IEnumerable<PurchaseOrder>>> GetPurchaseOrdersList()
        {
            return await _context.PurchaseOrders.ToListAsync();
        }



        [HttpGet("GetPurchaseOrder")]
        public async Task<ActionResult<PurchaseOrder>> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            return Ok(purchaseOrder);
        }


        [HttpPut("PutPurchaseOrder")]
        public async Task<IActionResult> PutPurchaseOrder(int id, [FromBody] PurchaseOrderDto purchaseOrderDto)
        {
          
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            purchaseOrder.Name = purchaseOrderDto.Name;
            purchaseOrder.OrderDate = purchaseOrderDto.OrderDate;
            purchaseOrder.DeliveryDate = purchaseOrderDto.DeliveryDate;
            purchaseOrder.Remarks = purchaseOrderDto.Remarks;
            purchaseOrder.Amouunt = purchaseOrderDto.Amouunt;
            purchaseOrder.SubTotal = purchaseOrderDto.SubTotal;
            purchaseOrder.Discount = purchaseOrderDto.Discount;
            purchaseOrder.Tax = purchaseOrderDto.Tax;
            purchaseOrder.Total = purchaseOrderDto.Total;

            _context.Entry(purchaseOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Purchase Order Updated successfully" });
        }
        [HttpPost("PostPurchaseOrder")]
        public async Task<ActionResult<PurchaseOrder>> PostPurchaseOrder([FromBody] PurchaseOrderDto purchaseOrderDto)
        {
            var purchaseOrder = new PurchaseOrder
            {
                Name = purchaseOrderDto.Name,
                OrderDate = purchaseOrderDto.OrderDate,
                DeliveryDate = purchaseOrderDto.DeliveryDate,
                Remarks = purchaseOrderDto.Remarks,
                Amouunt = purchaseOrderDto.Amouunt,
                SubTotal = purchaseOrderDto.SubTotal,
                Discount = purchaseOrderDto.Discount,
                Tax = purchaseOrderDto.Tax,
                Total = purchaseOrderDto.Total
            };
            _context.PurchaseOrders.Add(purchaseOrder);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Purchase Order Saved successfully" });
        }

        [HttpDelete("DeletePurchaseOrder")]
        public async Task<IActionResult> DeletePurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrders.FindAsync(id);
            _context.PurchaseOrders.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Purchase Order Deleted successfully" });
        }

        private bool PurchaseOrderExists(int id)
        {
            return _context.PurchaseOrders.Any(e => e.Id == id);
        }
    }
}
