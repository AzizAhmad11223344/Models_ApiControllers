using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using POSAPI.Data;
using POSAPI.DTOs;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly DataContext _db;

        public CategoryController(DataContext db)
        {
            _db = db;
        }
        //[HttpGet("GetCategoryList")]
        //public List<Category> GetCategoryList()
        //{
        //    var category = new Category
        //    {
        //        CreatedDate = DateTime.Now,
        //        Description = "Test description",
        //        Name = "Test name",
        //        Status = 1
        //    };

        //    _db.Categories.Add(category);

        //    _db.SaveChanges();


        //    var data = _db.Categories.ToList();
        //    return data;
        //}


        // GET: CategoryController
        [HttpGet("GetCategoryList")]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategoryList()
        {
            return await _db.Categories.ToListAsync();
        }

        [HttpGet("GetCategory")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            return Ok(category);
        }


        [HttpPost("PostCategory")]
        public async Task<ActionResult<Category>> PostCategory([FromBody] CategoryDto categoryDto)
        {
            var obj = new Category();
            obj.Name = categoryDto.Name;
            obj.Description = categoryDto.Description;
            obj.Status = 1;
            obj.CreatedDate=DateTime.Now;
            obj.CreatedBy = "Profile Required";
            _db.Categories.Add(obj);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Category Saved successfully." });
        }

        [HttpPut("PutCategory")]
        public async Task<IActionResult> PutCategory(int id,[FromBody] CategoryDto categoryDto)
        {
            var category = await _db.Categories.FindAsync(id);

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.Status = 1;
            category.ModifiedDate = DateTime.Now;
            category.ModifiedBy = "Profile Required";
            _db.Entry(category).State = EntityState.Modified;

            try
            {
               await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!CategoryExsists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
                
            }
            return Ok(new {message="Category Updated"});
        }

        private bool CategoryExsists(int id)
        {
            return _db.Categories.Any(x=>x.Id==id);
        }

        [HttpDelete("DeleteCategory")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = _db.Categories.FindAsync(id);
            _db.Categories.Remove(await category);
            await _db.SaveChangesAsync();
            return Ok(new { message = "Category Deleted successfully." });
        }



    }
}
