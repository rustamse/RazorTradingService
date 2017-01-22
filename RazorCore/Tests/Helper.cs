using System;
using System.Globalization;

namespace RazorCore.Tests
{
	class Helper
	{
		public static DateTime GenerateSubscrDate(string date)
		{
			var generateSubscrDate = DateTime.Parse(date, CultureInfo.InvariantCulture);
			return generateSubscrDate.Date;
		}
	}
}