using NUnit.Framework;
using RazorCore.Cash;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CostCalculatorTest
	{
		[Test]
		public void CalculateTotalCost_WhenEmptyHistory_Returns0()
		{
			var subscriptionHistory = SubscriptionHistoryBuilder.Create().Build();
			var calculator = new CostCalculator(subscriptionHistory);

			var calculateTotalCost = calculator.CalculateTotalCost();

			Assert.AreEqual(0, calculateTotalCost);
		}

		[Test]
		public void CalculateTotalCost_WhenHistoryHasOneDelivery_ReturnsCostOfOneDelivery()
		{
			var razorPricePerOneDelivery = 1;

			var subscriptionHistory = SubscriptionHistoryBuilder.Create()
				.WithInterval("1 jan 2017", "1 feb 2017", razorPricePerOneDelivery, "10 jan 2017")
				.Build();

			var calculator = new CostCalculator(subscriptionHistory);

			var calculateTotalCost = calculator.CalculateTotalCost();

			Assert.AreEqual(razorPricePerOneDelivery, calculateTotalCost);
		}

		[Test]
		public void CalculateTotalCost_WhenHistoryHasFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery_ReturnsCostOfThreeDelivery()
		{
			var razorPricePerOneMonth = 1;
			var razorAndGelPricePerOnemonth = 9;

			var subscriptionHistory = SubscriptionHistoryBuilder.Create()
				.WithInterval("1 jan 2017", "1 mar 2017", razorPricePerOneMonth, "15 jan 2017", "15 feb 2017")
				.WithInterval("2 mar 2017", "1 apr 2017", razorAndGelPricePerOnemonth, "20 mar 2017")
				.Build();

			var calculator = new CostCalculator(subscriptionHistory);

			var calculateTotalCost = calculator.CalculateTotalCost();

			Assert.AreEqual(2 * razorPricePerOneMonth + razorAndGelPricePerOnemonth, calculateTotalCost);
		}

		[Test]
		public void CalculateTotalCost_WhenDeliverySuspended_ReturnsZeroCost()
		{
			var razorAndGelAndFoamPricePerOnemonth = 19;

			var subscriptionHistory = SubscriptionHistoryBuilder.Create()
				.WithInterval("1 jan 2017", "1 jan 2018", razorAndGelAndFoamPricePerOnemonth)
				.Build();
			var calculator = new CostCalculator(subscriptionHistory);

			var calculateTotalCost = calculator.CalculateTotalCost();

			Assert.AreEqual(0, calculateTotalCost);
		}

		[Test]
		public void CalculateTotalCost_WhenDeliverySuspendAfterOneDeliveryAfterSuspended_ReturnsOneDeliveryCost()
		{
			var razorAndGelAndFoamPricePerOnemonth = 19;

			var subscriptionHistory = SubscriptionHistoryBuilder.Create()
				.WithInterval("1 jan 2017", "1 feb 2017", razorAndGelAndFoamPricePerOnemonth)
				.WithInterval("2 feb 2017", "1 mar 2017", razorAndGelAndFoamPricePerOnemonth, "16 feb 2017")
				.WithInterval("2 mar 2017", "1 apr 2017", razorAndGelAndFoamPricePerOnemonth)
				.Build();
			var calculator = new CostCalculator(subscriptionHistory);

			var calculateTotalCost = calculator.CalculateTotalCost();

			Assert.AreEqual(1 * razorAndGelAndFoamPricePerOnemonth, calculateTotalCost);
		}
	}
}