using System;
using System.Globalization;

namespace RazorCore.Tests
{
	static class DateTimeExtension
	{
		public static DateTime ToDate(this string date)
		{
			var generateSubscrDate = DateTime.Parse(date, CultureInfo.InvariantCulture);
			return generateSubscrDate.Date;
		}
	}
}