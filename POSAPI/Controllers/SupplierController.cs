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
    public class SupplierController : ControllerBase
    {
        private readonly DataContext _context;

        public SupplierController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("GetSuppliersList")]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliersList()
        {
            return await _context.Suppliers.ToListAsync();
        }
        [HttpGet("GetSupplier")]
        public async Task<ActionResult<Supplier>> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            return supplier;
        }
        [HttpPut("PutSupplier")]
        public async Task<IActionResult> PutSupplier(int id, [FromBody] SupplierDto supplierdto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            supplier.FirstName = supplierdto.FirstName;
            supplier.LastName = supplierdto.LastName;
            supplier.Email = supplierdto.Email;
            supplier.Phone = supplierdto.Phone;
            supplier.Telephone = supplierdto.Telephone;
            supplier.AddressLine1 = supplierdto.AddressLine1;
            supplier.AddressLine2 = supplierdto.AddressLine2;
            supplier.City = supplierdto.City;
            supplier.State = supplierdto.State;
            supplier.Country = supplierdto.Country;
            supplier.PostalCode = supplierdto.PostalCode;
            supplier.Status = 1;
            supplier.ModifiedDate = DateTime.Now;
            supplier.ModifiedBy = "Profile Required";
            _context.Entry(supplier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Info Updated successfully" });
        }
        [HttpPost("PostSupplier")]
        public async Task<ActionResult<Supplier>> PostSupplier([FromBody]SupplierDto supplierdto)
        {
            var supplier = new Supplier
            {
                FirstName = supplierdto.FirstName,
                LastName = supplierdto.LastName,
                Email = supplierdto.Email,
                Phone = supplierdto.Phone,
                Telephone = supplierdto.Telephone,
                AddressLine1 = supplierdto.AddressLine1,
                AddressLine2 = supplierdto.AddressLine2,
                City = supplierdto.City,
                State = supplierdto.State,
                Country = supplierdto.Country,
                PostalCode = supplierdto.PostalCode,
                Status = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = "Profile Required"
            };
            _context.Suppliers.Add(supplier);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Info Saved successfully" });
        }
        [HttpDelete("DeleteSupplier")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Info Deleted successfully" });
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
