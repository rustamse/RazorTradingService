using System.Collections.Generic;
using Moq;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	class PriceListBuilder
	{
		private readonly Mock<IPriceList> _priceList = new Mock<IPriceList>();

		private readonly Dictionary<SubscriptionTypes, double> _priceMap =
			new Dictionary<SubscriptionTypes, double>();

		public static PriceListBuilder Create()
		{
			return new PriceListBuilder();
		}

		public IPriceList Build()
		{
			_priceList.Setup(list => list.GetPrice(It.IsAny<SubscriptionTypes>()))
				.Returns<SubscriptionTypes>(sType => _priceMap[sType]);

			return _priceList.Object;
		}

		public PriceListBuilder WithPrice(SubscriptionTypes subscriptionType, double price)
		{
			_priceMap[subscriptionType] = price;
			return this;
		}
	}
}