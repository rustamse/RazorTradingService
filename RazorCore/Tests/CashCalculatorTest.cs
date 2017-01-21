using NUnit.Framework;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class CashCalculatorTest
	{
		[Test]
		public void CalculateTotalCash_WhenEmptyHistory_Returns0()
		{
			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create().Build();
			var priceList = PriceListBuilder.Create().Build();
			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(0, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenHistoryHasOneDelivery_ReturnsCostOfOneDelivery()
		{
			var razorPricePerOneDelivery = 1;

			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create()
				.WithInterval("1 jan 2017", "1 feb 2017", SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth, 10)
				.Build();
			var priceList = PriceListBuilder.Create()
				.WithPrice(SubscriptionTypes.Razor, razorPricePerOneDelivery)
				.Build();

			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(razorPricePerOneDelivery, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenHistoryHasFirstItemWith2DeliveryAndSecondItemWithOtherOneDelivery_ReturnsCostOfThreeDelivery()
		{
			var razorPricePerOneMonth = 1;
			var razorAndGelPricePerOnemonth = 9;

			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create()
				.WithInterval("1 jan 2017", "1 mar 2017", SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth, 10)
				.WithInterval("2 mar 2017", "1 apr 2017", SubscriptionTypes.RazorAndGel, DeliveryRegularity.OncePerMonth, 5)
				.Build();
			var priceList = PriceListBuilder.Create()
				.WithPrice(SubscriptionTypes.Razor, razorPricePerOneMonth)
				.WithPrice(SubscriptionTypes.RazorAndGel, razorAndGelPricePerOnemonth)
				.Build();
			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(2 * razorPricePerOneMonth + razorAndGelPricePerOnemonth, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenHistoryHasOneYearAndHasDeliveryOncePerTwoMonths_ReturnsCostOfSixDelivery()
		{
			var razorAndGelAndFoamPricePerOnemonth = 19;

			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create()
				.WithInterval("1 jan 2017", "1 jan 2018", SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.OncePerTwoMonths, 2)
				.Build();
			var priceList = PriceListBuilder.Create()
				.WithPrice(SubscriptionTypes.RazorAndGelAndFoam, razorAndGelAndFoamPricePerOnemonth)
				.Build();
			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(6 * razorAndGelAndFoamPricePerOnemonth, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenDeliverySuspended_ReturnsZeroCost()
		{
			var razorAndGelAndFoamPricePerOnemonth = 19;

			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create()
				.WithInterval("1 jan 2017", "1 jan 2018", SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.Suspended, 2)
				.Build();
			var priceList = PriceListBuilder.Create()
				.WithPrice(SubscriptionTypes.RazorAndGelAndFoam, razorAndGelAndFoamPricePerOnemonth)
				.Build();
			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(0, resultCash);
		}

		[Test]
		public void CalculateTotalCash_WhenDeliverySuspendAfterPresentAfterSuspended_ReturnsOneDeliveryCost()
		{
			var razorAndGelAndFoamPricePerOnemonth = 19;

			var cashIntervalsProvider = CashIntervalsProviderBuilder.Create()
				.WithInterval("1 jan 2017", "1 feb 2017", SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.Suspended, 15)
				.WithInterval("2 feb 2017", "1 mar 2017", SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.OncePerMonth, 16)
				.WithInterval("2 mar 2017", "1 apr 2017", SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.Suspended, 18)
				.Build();
			var priceList = PriceListBuilder.Create()
				.WithPrice(SubscriptionTypes.RazorAndGelAndFoam, razorAndGelAndFoamPricePerOnemonth)
				.Build();
			var cashCalculator = new CashCalculator(cashIntervalsProvider, priceList);

			var resultCash = cashCalculator.CalculateTotalCash();

			Assert.AreEqual(1 * razorAndGelAndFoamPricePerOnemonth, resultCash);
		}
	}
}