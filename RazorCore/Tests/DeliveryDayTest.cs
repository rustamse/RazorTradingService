using System;
using NUnit.Framework;
using RazorCore.Subscription;

namespace RazorCore.Tests
{
	[TestFixture]
	public class DeliveryDayTest
	{
		[Test]
		public void Ctor_WhenDayOfMonth5_ReturnsDayOfMonth5()
		{
			var expectedDayOfMonth = 5;

			var deliveryDay = new DeliveryDay(expectedDayOfMonth);

			Assert.AreEqual(expectedDayOfMonth, deliveryDay.DayOfMonth);
		}

		[Test]
		public void Ctor_WhenDayOfMonth0_ThrowsArgumentOutOfRangeException()
		{
			var expectedDayOfMonth = 0;

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryDay = new DeliveryDay(expectedDayOfMonth);
			});
		}

		[Test]
		public void Ctor_WhenDayOfMonth29_ThrowsArgumentOutOfRangeException()
		{
			var expectedDayOfMonth = 29;

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var deliveryDay = new DeliveryDay(expectedDayOfMonth);
			});
		}
	}
}