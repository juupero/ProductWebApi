using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Entities;

namespace ProductWebApi.Controllers
{
	[Route("api/[controller]/[Action]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{
		private readonly ProductDbContext _context;

		public CategoriesController(ProductDbContext context)
		{
			_context = context;
		}

		// GET: api/Categories
		[HttpGet]
		public ActionResult<IEnumerable<Category>> GetCategories()
		{
			return Ok(_context.Categories.ToArray());
		}

		// GET: api/Categories/5
		[HttpGet("{id}", Name = "GetCategory")]
		public async Task<ActionResult<Category>> GetCategory([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var category = await _context.Categories.FindAsync(id);

			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Category>> GetCategoryAndProducts([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var category = await _context.Categories.Include(c=>c.Products).SingleOrDefaultAsync(c=>c.Id == id);

			if (category == null)
			{
				return NotFound();
			}

			return Ok(category);
		}

		// PUT: api/Categories/5
		[HttpPut("{id}")]
		public async Task<ActionResult<Category>> PutCategory([FromRoute] int id, [FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			if (id != category.Id)
			{
				return BadRequest();
			}

			_context.Entry(category).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CategoryExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(category);
		}

		// POST: api/Categories
		[HttpPost]
		public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			_context.Categories.Add(category);
			await _context.SaveChangesAsync();

			return CreatedAtRoute("GetCategory", new { id = category.Id }, category);
		}

		// DELETE: api/Categories/5
		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteCategory([FromRoute] int id)
		//{
		//    if (!ModelState.IsValid)
		//    {
		//        return BadRequest(ModelState);
		//    }

		//    var category = await _context.Categories.FindAsync(id);
		//    if (category == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.Categories.Remove(category);
		//    await _context.SaveChangesAsync();

		//    return Ok(category);
		//}

		private bool CategoryExists(int id)
		{
			return _context.Categories.Any(e => e.Id == id);
		}
	}
}