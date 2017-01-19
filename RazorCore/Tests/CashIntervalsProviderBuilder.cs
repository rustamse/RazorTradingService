using System.Collections.Generic;
using Moq;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	class CashIntervalsProviderBuilder
	{
		private readonly Mock<ICashIntervalsProvider> _mock = new Mock<ICashIntervalsProvider>();

		private readonly List<CashInterval> _intervals = new List<CashInterval>();

		public static CashIntervalsProviderBuilder Create()
		{
			return new CashIntervalsProviderBuilder();
		}

		public ICashIntervalsProvider Build()
		{
			_mock.Setup(subscriptionHistory => subscriptionHistory.GetIntervals())
				.Returns(() => _intervals);

			return _mock.Object;
		}

		public CashIntervalsProviderBuilder WithInterval(string fromDate, string toDate,
			SubscriptionTypes subscriptionType, DeliveryRegularity deliveryRegularity, params int[] deliveryDays)
		{
			var subscriptionPlan = new SubscriptionPlan(subscriptionType,
				deliveryRegularity, new DeliveryTime(deliveryDays));
			var cashInterval = new CashInterval(subscriptionPlan, Helper.GenerateSubscrDate(fromDate),
				Helper.GenerateSubscrDate(toDate));
			_intervals.Add(cashInterval);

			return this;
		}
	}
}