using System;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorWpf
{
	public class PriceList : IPriceList
	{
		private readonly double _razorPrice;
		private readonly double _razorAndGelPrice;
		private readonly double _razorAndGelAndFoamPrice;

		public PriceList(double razorPrice, double razorAndGelPrice, double razorAndGelAndFoamPrice)
		{
			_razorPrice = razorPrice;
			_razorAndGelPrice = razorAndGelPrice;
			_razorAndGelAndFoamPrice = razorAndGelAndFoamPrice;
		}

		public double GetPrice(SubscriptionTypes subscriptionType)
		{
			if (subscriptionType == SubscriptionTypes.Razor)
				return _razorPrice;
			if (subscriptionType == SubscriptionTypes.RazorAndGel)
				return _razorAndGelPrice;
			if (subscriptionType == SubscriptionTypes.RazorAndGelAndFoam)
				return _razorAndGelAndFoamPrice;

			throw new ArgumentOutOfRangeException(nameof(subscriptionType));
		}
	}
}