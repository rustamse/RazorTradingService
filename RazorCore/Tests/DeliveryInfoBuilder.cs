using System;
using System.Collections.Generic;
using Moq;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	class DeliveryInfoBuilder
	{
		private readonly List<DateTime> _deliveryDays = new List<DateTime>();

		public static DeliveryInfoBuilder Create()
		{
			return new DeliveryInfoBuilder();
		}

		public IDeliveryInfo Build()
		{
			var deliveryInfo = new Mock<IDeliveryInfo>();
			deliveryInfo.Setup(info => info.IsDeliveryDay(It.IsAny<DateTime>()))
				.Returns<DateTime>(time => _deliveryDays.Contains(time.Date));
			return deliveryInfo.Object;
		}

		public DeliveryInfoBuilder WithDeliveryDay(DateTime deliveryDate)
		{
			_deliveryDays.Add(deliveryDate);
			return this;
		}
	}
}