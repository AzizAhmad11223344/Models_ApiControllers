using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using POSAPI.Data;
using POSAPI.DTOs;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Product
        [HttpGet("GetProductList")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductList()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Product/5
        [HttpGet("GetProduct")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            var productdto = new ProductDto
            {
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
            };

            return Ok(productdto);
        }
        // PUT: api/Product/5
        [HttpPut("PutProduct")]
        public async Task<IActionResult> PutProduct(int id,[FromBody] ProductDto producdto)
        {
            var product = await _context.Products.FindAsync(id);
            product.Name=producdto.Name;
            product.Price=producdto.Price;
            product.StockQuantity=producdto.StockQuantity;
           product.CategoryId=producdto.CategoryId;
            product.ModifiedDate = DateTime.Now;
            product.ModifiedBy = "Required Profile";

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(new {message= "Product Updated successfully" });
        }

        //// POST: api/Product
        [HttpPost("PostProduct")]
        public async Task<ActionResult<Product>> PostProduct([FromBody]ProductDto productDto)
        {
            var obj = new Product();
            obj.Name= productDto.Name;
            obj.Price= productDto.Price;
            obj.StockQuantity= productDto.StockQuantity;
            obj.CategoryId= productDto.CategoryId;
            obj.CreatedDate= DateTime.Now;
            obj.DeletedBy = "Required Profile";
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == obj.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("Invalid CategoryId.");
            }
            _context.Products.Add(obj);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Product Saved successfully" });
        }

        // DELETE: api/Product/5
        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new {message= "Product Deleted successfully" });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
