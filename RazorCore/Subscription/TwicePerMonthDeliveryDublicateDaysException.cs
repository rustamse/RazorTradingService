using System;

namespace RazorCore.Subscription
{
	public class TwicePerMonthDeliveryDublicateDaysException : Exception
	{
		public TwicePerMonthDeliveryDublicateDaysException(string msg)
			: base(msg)
		{

		}
	}
}