using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Moq;
using RazorCore.History;

namespace RazorCore.Tests
{
	class SubscriptionHistoryBuilder
	{
		private readonly Mock<ISubscriptionHistory> _mock = new Mock<ISubscriptionHistory>();

		private readonly List<ISubscriptionInterval> _intervals = new List<ISubscriptionInterval>();

		public static SubscriptionHistoryBuilder Create()
		{
			return new SubscriptionHistoryBuilder();
		}

		public ISubscriptionHistory Build()
		{
			_mock.Setup(subscriptionHistory => subscriptionHistory.GetHistory())
				.Returns(() => new ReadOnlyCollection<ISubscriptionInterval>(_intervals));

			return _mock.Object;
		}

		public SubscriptionHistoryBuilder WithInterval(string fromDate, string toDate,
			double productPrice, params string[] deliveryDays)
		{
			var interval = new Mock<ISubscriptionInterval>();
			interval.Setup(subscriptionInterval => subscriptionInterval.FromDate)
				.Returns(fromDate.ToDate());
			interval.Setup(subscriptionInterval => subscriptionInterval.ToDate)
				.Returns(toDate.ToDate());
			interval.Setup(subscriptionInterval => subscriptionInterval.GetOneDeliveryCost())
				.Returns(productPrice);
			interval.Setup(subscriptionInterval => subscriptionInterval.GetDeliveryDates())
				.Returns(deliveryDays.ToList().Select(DateTimeExtension.ToDate).ToList());

			_intervals.Add(interval.Object);

			return this;
		}
	}
}