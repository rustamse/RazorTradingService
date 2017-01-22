using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class ProductInfoTest
	{
		[Test]
		public void Ctor_WhenProductTypeRazor_ReturnsProductTypeRazor()
		{
			var expectedType = ProductTypes.Razor;

			var productInfo = new ProductInfo(expectedType, 10);

			Assert.AreEqual(expectedType, productInfo.ProductType);
		}

		[Test]
		public void Ctor_WhenPrice9_ReturnsPrice9()
		{
			var expectedPrice = 9;

			var productInfo = new ProductInfo(ProductTypes.RazorAndGel, expectedPrice);

			Assert.AreEqual(expectedPrice, productInfo.Price);
		}

		[Test]
		public void Ctor_WhenPriceZero_ThrowsArgumentOutOfrangeException()
		{
			var expectedPrice = 0;

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var productInfo = new ProductInfo(ProductTypes.Razor, expectedPrice);
			});
		}

		[Test]
		public void Ctor_WhenPriceMinus1_ThrowsArgumentOutOfrangeException()
		{
			var expectedPrice = -1;

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var productInfo = new ProductInfo(ProductTypes.Razor, expectedPrice);
			});
		}
	}
}