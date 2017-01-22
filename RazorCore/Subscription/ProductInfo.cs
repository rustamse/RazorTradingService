using System;

namespace RazorCore.Subscription
{
	public class ProductInfo : IProductInfo
	{
		public ProductTypes ProductType { get; }

		public double Price { get; }

		public ProductInfo(ProductTypes productType, double price)
		{
			if (price <= 0)
				throw new ArgumentOutOfRangeException(nameof(price));

			ProductType = productType;
			Price = price;
		}

	}
}