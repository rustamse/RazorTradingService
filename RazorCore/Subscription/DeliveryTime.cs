using System;

namespace RazorCore.Subscription
{
	public class DeliveryTime
	{
		public readonly int[] DeliveryDays;

		public DeliveryTime(params int[] deliveryDays)
		{
			if (deliveryDays == null)
				throw new ArgumentNullException(nameof(deliveryDays));

			DeliveryDays = deliveryDays;
		}
	}
}