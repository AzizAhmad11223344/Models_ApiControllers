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
    public class SaleOrderController : ControllerBase
    {
        private readonly DataContext _context;

        public SaleOrderController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("GetSaleOrdersList")]
        public async Task<ActionResult<IEnumerable<SaleOrder>>> GetSaleOrdersList()
        {
            return await _context.SaleOrders.ToListAsync();
        }
        [HttpGet("GetSaleOrder")]
        public async Task<ActionResult<SaleOrder>> GetSaleOrder(int id)
        {
            var saleOrder = await _context.SaleOrders.FindAsync(id);
            return saleOrder;
        }
        [HttpPut("PutSaleOrder")]
        public async Task<IActionResult> PutSaleOrder(int id,[FromBody] SaleOrderDto saleOrderdto)
        {
            var saleOrder = await _context.SaleOrders.FindAsync(id);
            saleOrder.Name = saleOrderdto.Name;
            saleOrder.OrderDate = saleOrderdto.OrderDate;
            saleOrder.DeliveryDate = saleOrderdto.DeliveryDate;
            saleOrder.Remarks = saleOrderdto.Remarks;
            saleOrder.Amouunt = saleOrderdto.Amouunt;
            saleOrder.SubTotal = saleOrderdto.SubTotal;
            saleOrder.Discount = saleOrderdto.Discount;
            saleOrder.Tax = saleOrderdto.Tax;
            saleOrder.Total = saleOrderdto.Total;
            _context.Entry(saleOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }


            return Ok(new { message = "Sale Order Updated successfully" });
        }
        [HttpPost("PostSaleOrder")]
        public async Task<ActionResult<SaleOrder>> PostSaleOrder([FromBody] SaleOrderDto saleOrderdto)
        {
            var saleorder = new SaleOrder
            {
                Name = saleOrderdto.Name,
                OrderDate = saleOrderdto.OrderDate,
                DeliveryDate = saleOrderdto.DeliveryDate,
                Remarks = saleOrderdto.Remarks,
                Amouunt = saleOrderdto.Amouunt,
                SubTotal = saleOrderdto.SubTotal,
                Discount = saleOrderdto.Discount,
                Tax = saleOrderdto.Tax,
                Total = saleOrderdto.Total
            };
            _context.SaleOrders.Add(saleorder);
            await _context.SaveChangesAsync();


            return Ok(new { message = "Sale Order Saved successfully" });
        }
        [HttpDelete("DeleteSaleOrder")]
        public async Task<IActionResult> DeleteSaleOrder(int id)
        {
            var saleOrder = await _context.SaleOrders.FindAsync(id);
            _context.SaleOrders.Remove(saleOrder);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Sale Order Deleted successfully" });
        }

        private bool SaleOrderExists(int id)
        {
            return _context.SaleOrders.Any(e => e.Id == id);
        }
    }
}
