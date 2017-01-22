using System;

namespace RazorCore.Subscription
{
	public class OncePerMonthDelivery : IDeliveryInfo
	{
		private readonly DeliveryDay _deliveryDay;

		public OncePerMonthDelivery(DeliveryDay deliveryDay)
		{
			if (deliveryDay == null)
				throw new ArgumentNullException(nameof(deliveryDay));

			_deliveryDay = deliveryDay;
		}

		public bool IsDeliveryDay(DateTime checkDate)
		{
			var isDeliveryDayEqualsCheckDay = _deliveryDay.DayOfMonth == checkDate.Day;
			return isDeliveryDayEqualsCheckDay;
		}
	}
}