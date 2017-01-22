namespace RazorCore.Subscription
{
	public class ProductInfo : IProductInfo
	{
		public ProductTypes ProductType { get; }

		public double Price { get; }

		public ProductInfo(ProductTypes productType, double price)
		{
			ProductType = productType;
			Price = price;
		}

	}
}