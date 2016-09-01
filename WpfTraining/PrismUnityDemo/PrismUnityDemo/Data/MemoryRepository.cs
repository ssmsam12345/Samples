﻿using PrismUnityDemo.Contracts;
using System.Linq;

namespace PrismUnityDemo.Data
{
    public class MemoryRepository : Repository
	{
		private Product[] sampleProducts = new[] {
			new Product() { ProductNumber = 1, ProductName = "Twisted Drill" },
			new Product() { ProductNumber = 2, ProductName = "Indexable Drill" },
			new Product() { ProductNumber = 3, ProductName = "Slot Miller" }
		};

		public override IQueryable<Product> SelectAllProducts()
		{
			return this.sampleProducts.AsQueryable();
		}
	}
}
