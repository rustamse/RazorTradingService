using System;

namespace RazorCore.Subscription
{
	public class OncePerTwoMonthsDelivery : IDeliveryInfo
	{
		private readonly DeliveryDay _deliveryDay;

		public OncePerTwoMonthsDelivery(DeliveryDay deliveryDay)
		{
			if (deliveryDay == null)
				throw new ArgumentNullException(nameof(deliveryDay));

			_deliveryDay = deliveryDay;
		}

		public string GetDescription()
		{
			return "OncePerTwoMonthsDelivery";
		}

		public bool IsDeliveryDay(DateTime checkDate)
		{
			var isEvenMonth = checkDate.Month % 2 == 0;
			if (isEvenMonth)
				return false;

			var isDeliveryDayEqualsCheckDay = _deliveryDay.DayOfMonth == checkDate.Day;
			return isDeliveryDayEqualsCheckDay;
		}
	}
}