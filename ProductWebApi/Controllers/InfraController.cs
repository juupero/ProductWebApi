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
	public class InfraController : ControllerBase
	{
		private readonly ProductDbContext _context;

		public InfraController(ProductDbContext context)
		{
			_context = context;
		}
        [HttpGet]
        public string AddTestData(int categoryCount, int productCount)
        {
            for (int i = 0; i < categoryCount; i++)
            {
                var newCat = new Category { Name = "Test nb" + i, Products = new List<Product>() };
                for (int j = 0; j < productCount; j++)
                {
                    newCat.Products.Add(new Product { Name = "TestProduct nb " + j, Price = j * 100 });
                }
                _context.Categories.Add(newCat);
                _context.SaveChanges();
            }
            return _context.Database.GetDbConnection().ConnectionString;
        }
        //[HttpGet]
        //public string GetDbConnectionString()
        //{
        //	return _context.Database.GetDbConnection().ConnectionString;
        //}


        [HttpPost]
		public async Task<ActionResult<string>> CreateDatabase([FromBody] string key)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (key != "Oikeesti") return BadRequest("Key must be 'Oikeesti'");

			await _context.Database.EnsureDeletedAsync();
			await _context.Database.EnsureCreatedAsync();

			_context.Categories.Add(new Category
				{
					Name="Eka kategoria",
					Products = new List<Product>
						{
							new Product{Name="Hila", Price=10},
							new Product{Name="Vitkutin", Price=100},
							new Product{Name="Hilavitkutin", Price=1000},
						}
				});
			await _context.SaveChangesAsync();

			return Ok("Created a new DB");
		}

	}
}