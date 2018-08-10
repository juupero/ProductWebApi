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
    public class ProductsController : ControllerBase
    {
        private readonly ProductDbContext _context;

        public ProductsController(ProductDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetProducts(int? pageNb, int? pageSize)
        {
            //tarkistetaan ja asetetaan pageNb ja pageSize sallittuihin arvoihin. Ensimmäisen sivun nro on 0.
            if (pageNb == null || pageNb.Value < 0) pageNb = 0;
            if (pageSize == null || pageSize.Value < 1 || pageSize.Value > 100) pageSize = 100;

            // koska pageNb ja pageSize on voinut muuttua niistä, jotka client antoi, laitetaan kätettävät arvot myös headeriin
            Response.Headers.Add("x-pageNb", pageNb.ToString());
            Response.Headers.Add("x-pageSize", pageSize.ToString());

            // tuotteiden lukumäärä laitetaan headeriin
            Response.Headers.Add("x-productCount", _context.Products.Count().ToString());

            return Ok(
               _context.Products
                      .OrderBy(p => p.Id)
                      .Skip(pageNb.Value * pageSize.Value)
                      .Take(pageSize.Value)
                      .ToArray()
                );
        }


        // GET: api/Products/5
        [HttpGet("{id}", Name = "GetProduct")]
		public async Task<ActionResult<Product>> GetProduct([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				return NotFound();
			}

			return Ok(product);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<(Product Product, string CategoryName)>> GetProductAndCategory([FromRoute] int id)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var product = await _context.Products.FindAsync(id);

			if (product == null)
			{
				return NotFound();
			}
			var category = await _context.Categories.FindAsync(product.CategoryId);
			return Ok((Product:product,CategoryName:category.Name));
		}

		// PUT: api/Products/5
		[HttpPut("{id}")]
        public async Task<ActionResult<Product>> PutProduct([FromRoute] int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

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

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}