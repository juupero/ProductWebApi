using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
	public class ProductDbContext : DbContext
	{
		public ProductDbContext(DbContextOptions<ProductDbContext> options): base(options) {	}
		public ProductDbContext()	{	}

		public DbSet<Category> Categories { get; set; }
		public DbSet<Product> Products { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Category>()
			.ToTable("Category")
			.HasMany(c => c.Products)
			.WithOne()
			;

			modelBuilder.Entity<Product>()
				.ToTable("Product")
				;

		

		}
	}
}
