using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.DTOs;
using POSAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;

        public CustomerController(DataContext context)
        {
            _context = context;
        }
        [HttpGet("GetCustomerList")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomerList()
        {
            var customers = await _context.Customers.ToListAsync();
            return Ok(customers);
        }
        [HttpGet("GetCustomer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);

            return Ok(customer);
        }
        [HttpPost("PostCustomer")]
        public async Task<ActionResult> PostCustomer([FromBody] CustomerDto customerDto)
        {
            var customer = new Customer
            {
                FirstName = customerDto.FirstName,
                LastName = customerDto.LastName,
                Email = customerDto.Email,
                Phone = customerDto.Phone,
                Telephone = customerDto.Telephone,
                AddressLine1 = customerDto.AddressLine1,
                AddressLine2 = customerDto.AddressLine2,
                City = customerDto.City,
                State = customerDto.State,
                Country = customerDto.Country,
                PostalCode = customerDto.PostalCode,
                Status = 1,
                CreatedDate = DateTime.Now,
                CreatedBy = "Profile Required"
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Customer Saved" });
        }
        [HttpPut("PutCustomer")]
        public async Task<IActionResult> PutCustomer(int id,[FromBody]  CustomerDto customerDto)
        {
            var customer = await _context.Customers.FindAsync(id);
            customer.FirstName = customerDto.FirstName;
            customer.LastName = customerDto.LastName;
            customer.Email = customerDto.Email;
            customer.Phone = customerDto.Phone;
            customer.Telephone = customerDto.Telephone;
            customer.AddressLine1 = customerDto.AddressLine1;
            customer.AddressLine2 = customerDto.AddressLine2;
            customer.City = customerDto.City;
            customer.State = customerDto.State;
            customer.Country = customerDto.Country;
            customer.PostalCode = customerDto.PostalCode;
            customer.Status = 1;
            customer.ModifiedDate = DateTime.Now;
            customer.ModifiedBy = "Profile Required";

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new { message = "Customer Updated" });
        }
        [HttpDelete("DeleteCustomer")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Customer deleted successfully." });
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
