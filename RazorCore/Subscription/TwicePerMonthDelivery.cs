using System;

namespace RazorCore.Subscription
{
	public class TwicePerMonthDelivery : IDeliveryInfo
	{
		private readonly DeliveryDay _deliveryFirstDay;
		private readonly DeliveryDay _deliverySecondDay;

		public TwicePerMonthDelivery(DeliveryDay deliveryFirstDay, DeliveryDay deliverySecondDay)
		{
			if (deliveryFirstDay == null)
				throw new ArgumentNullException(nameof(deliveryFirstDay));
			if (deliverySecondDay == null)
				throw new ArgumentNullException(nameof(deliverySecondDay));

			if (deliveryFirstDay.DayOfMonth == deliverySecondDay.DayOfMonth)
				throw new TwicePerMonthDeliveryDublicateDaysException("deliveryFirstDay == deliverySecondDay");

			_deliveryFirstDay = deliveryFirstDay;
			_deliverySecondDay = deliverySecondDay;
		}

		public bool IsDeliveryDay(DateTime checkDate)
		{
			var isDeliveryDay1EqualsCheckDay = _deliveryFirstDay.DayOfMonth == checkDate.Day;
			var isDeliveryDay2EqualsCheckDay = _deliverySecondDay.DayOfMonth == checkDate.Day;

			return isDeliveryDay1EqualsCheckDay || isDeliveryDay2EqualsCheckDay;
		}
	}
}