using System;
using System.Linq;
using NUnit.Framework;
using RazorCore.History;

namespace RazorCore.Tests
{
	[TestFixture]
	public class SubscriptionIntervalTest
	{
		[Test]
		public void Ctor_WhenArgumentProductIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var deliveryInfo = DeliveryInfoBuilder.Create().Build();
				var fromDate = "1 jan 2017".ToDate();
				var toDate = "1 feb 2017".ToDate();
				// ReSharper disable once UnusedVariable
				var interval = new SubscriptionInterval(null, deliveryInfo, fromDate, toDate);
			});
		}

		[Test]
		public void Ctor_WhenArgumentDeliveryIsNull_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() =>
			{
				var productInfo = ProductInfoBuilder.Create().Build();
				var fromDate = "1 jan 2017".ToDate();
				var toDate = "1 feb 2017".ToDate();
				// ReSharper disable once UnusedVariable
				var interval = new SubscriptionInterval(productInfo, null, fromDate, toDate);
			});
		}

		[Test]
		public void Ctor_WhenArgumentToDateSmallerFromDate_ThrowsArgumentOutOfRangeException()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				var deliveryInfo = DeliveryInfoBuilder.Create().Build();
				var productInfo = ProductInfoBuilder.Create().Build();
				var fromDate = "1 feb 2017".ToDate();
				var toDate = "1 jan 2017".ToDate();
				// ReSharper disable once UnusedVariable
				var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);
			});
		}

		[Test]
		public void FromDate_WhenCtorArgumentFromJan1_ReturnsJan1()
		{
			var fromDate = "1 jan 2017".ToDate();

			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var productInfo = ProductInfoBuilder.Create().Build();
			var toDate = "1 feb 2017".ToDate();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			Assert.AreEqual(fromDate, interval.FromDate);
		}

		[Test]
		public void ToDate_WhenCtorArgumentToFeb1_ReturnsFeb1()
		{
			var toDate = "1 feb 2017".ToDate();

			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var productInfo = ProductInfoBuilder.Create().Build();
			var fromDate = "1 jan 2017".ToDate();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			Assert.AreEqual(toDate, interval.ToDate);
		}

		[Test]
		public void ToDate_WHenModifyToDateToFeb2_ReturnsFeb2()
		{
			var newToDate = "2 feb 2017".ToDate();

			var fromDate = "1 jan 2017".ToDate();
			var toDate = "1 feb 2017".ToDate();
			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			interval.ModifyToDate(newToDate);

			Assert.AreEqual(newToDate, interval.ToDate);
		}

		[Test]
		public void GetOneDeliveryPrice_WhenProductPrice10_Returns10()
		{
			var productPrice = 10;
			var productInfo = ProductInfoBuilder.Create()
				.WithPrice(productPrice)
				.Build();

			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var fromDate = "1 jan 2017".ToDate();
			var toDate = "1 feb 2017".ToDate();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			var resultCost = interval.GetOneDeliveryCost();

			Assert.AreEqual(productPrice, resultCost);
		}

		[Test]
		public void GetDeliveryDates_WhenDeliveryAtJan5And25_ReturnsJan5And25()
		{
			var fromDate = "1 jan 2017".ToDate();
			var toDate = "1 feb 2017".ToDate();
			var deliveryFirstDate = "5 jan 2017".ToDate();
			var deliverySecondDate = "25 jan 2017".ToDate();

			var productInfo = ProductInfoBuilder.Create().Build();
			var deliveryInfo = DeliveryInfoBuilder.Create()
				.WithDeliveryDay(deliveryFirstDate)
				.WithDeliveryDay(deliverySecondDate)
				.Build();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			var deliveryDates = interval.GetDeliveryDates();

			Assert.AreEqual(deliveryFirstDate, deliveryDates.First());
			Assert.AreEqual(deliverySecondDate, deliveryDates.Last());
		}

		[Test]
		public void CreateCopy_WhenModifyToDateToFeb2_ReturnsFeb2()
		{
			var price = 6;
			var fromDate = "1 jan 2017".ToDate();
			var toDate = "1 feb 2017".ToDate();
			var productInfo = ProductInfoBuilder.Create()
				.WithPrice(price)
				.Build();
			var deliveryInfo = DeliveryInfoBuilder.Create().Build();
			var interval = new SubscriptionInterval(productInfo, deliveryInfo, fromDate, toDate);

			var copy = interval.CreateCopy();

			Assert.AreEqual(toDate, copy.ToDate);
			Assert.AreEqual(fromDate, copy.FromDate);
			Assert.AreEqual(price, copy.GetOneDeliveryCost());
		}
	}
}