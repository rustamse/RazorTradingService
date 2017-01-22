using Moq;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	class ProductInfoBuilder
	{
		private double _price;

		public static ProductInfoBuilder Create()
		{
			return new ProductInfoBuilder();
		}

		public IProductInfo Build()
		{
			var m = new Mock<IProductInfo>();
			m.Setup(info => info.Price)
				.Returns(_price);
			return m.Object;
		}

		public ProductInfoBuilder WithPrice(double price)
		{
			_price = price;
			return this;
		}
	}
}