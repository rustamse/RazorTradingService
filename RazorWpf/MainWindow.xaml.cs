using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RazorCore.History;
using RazorCore.Subscription;

namespace RazorWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow
	{
		private CalenderBackground _calendarBackground;

		private readonly ISubscriptionHistory _subscriptionHistory = new SubscriptionHistory();
		private const int RazorPrice = 1;
		private const int RazorAndGelPrice = 9;
		private const int RazorAndGelAndFoamPrice = 19;

		public MainWindow()
		{
			InitializeComponent();

			for (int i = 1; i <= 28; i++)
			{
				DeliveryOncePer2MonthsComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryOncePerMonthComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryTwicePerMonthComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				DeliveryTwicePerMonthComboBox2.Items.Add(new ComboBoxItem { Content = i.ToString() });
			}

			DeliveryOncePer2MonthsComboBox.SelectedIndex = 0;
			DeliveryOncePerMonthComboBox.SelectedIndex = 0;
			DeliveryTwicePerMonthComboBox.SelectedIndex = 0;
			DeliveryTwicePerMonthComboBox2.SelectedIndex = 14;

			Calendar.DisplayDateChanged += CalendarOnDisplayDateChanged;

			Calendar.SelectedDatesChanged += CalendarOnSelectedDatesChanged;
			Calendar.SelectedDate = DateTime.Now.Date;

			Update();
		}

		private void CalendarOnDisplayDateChanged(object sender, CalendarDateChangedEventArgs calendarDateChangedEventArgs)
		{
			Update();
		}

		private void CalendarOnSelectedDatesChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
			Update();
		}

		private void OnMakeSubscriptionClick(object sender, RoutedEventArgs e)
		{
			try
			{
				var productInfo = ParseProductInfo();
				var deliveryInfo = ParseDeliveryInfo();
				var selectedDate = ParseSelectedDate();

				_subscriptionHistory.AddSubscription(productInfo, deliveryInfo, selectedDate);

				Update();
			}
			catch (TwicePerMonthDeliveryDublicateDaysException ex)
			{
				MessageBox.Show(ex.Message, "Неверно выбраны дни доставки.");
			}
			catch (SubscriptionHistoryOldIntervalException)
			{
				MessageBox.Show("Нельзя изменить подписку для архивной подписки", "Неверно изменена подписка.");
			}
		}

		private void Update()
		{
			var calendarSelectedDate = ParseSelectedDate();
			_subscriptionHistory.UpdateSubscriptionEndDate(calendarSelectedDate);

			_calendarBackground = new CalenderBackground(Calendar);
			_calendarBackground.AddOverlay("circle", "circle.png");
			_calendarBackground.AddOverlay("tjek", "tjek.png");
			_calendarBackground.AddOverlay("cross", "cross.png");
			_calendarBackground.AddOverlay("box", "box.png");
			_calendarBackground.AddOverlay("gray", "gray.png");

			_calendarBackground.grayoutweekends = "gray";

			Calendar.Background = _calendarBackground.GetBackground(calendarSelectedDate);

			var subscriptionIntervals = _subscriptionHistory.GetHistory();
			if (subscriptionIntervals.Any())
			{
				var copy = subscriptionIntervals.Last().CreateCopy();
				copy.ModifyToDate(copy.ToDate.AddYears(10));
				foreach (var deliveryDay in copy.GetDeliveryDates())
				{
					_calendarBackground.AddDate(deliveryDay, "circle");
				}
			}

			var costCalculator = new CostCalculator(_subscriptionHistory);

			Calendar.Background = _calendarBackground.GetBackground(calendarSelectedDate);
			CashTxt.Text = ((int)costCalculator.CalculateTotalCost()).ToString();

			MakeSubscription.Content = _subscriptionHistory.GetHistory().Any() ?
				$"Изменить подписку с {calendarSelectedDate.ToString("d")}" :
				$"Оформить подписку с {calendarSelectedDate.ToString("d")}";

			DateTxt.Text = $"Выбор даты (текущая дата: {calendarSelectedDate.ToString("d")})";

			DrawSubscriptionHistory();
		}

		private void DrawSubscriptionHistory()
		{
			SubscrHistoryList.Items.Clear();
			foreach (var subscriptionInterval in _subscriptionHistory.GetHistory())
			{
				SubscrHistoryList.Items.Add($"{subscriptionInterval.GetDescription()}");
			}
		}

		private DateTime ParseSelectedDate()
		{
			// ReSharper disable once PossibleInvalidOperationException
			return Calendar.SelectedDate.Value.Date;
		}

		private IDeliveryInfo ParseDeliveryInfo()
		{
			if (DeliveryOncePer2MonthsCheckBox.IsChecked == true)
			{
				var deliveryDay = new DeliveryDay(DeliveryOncePer2MonthsComboBox.SelectedIndex + 1);
				return new OncePerTwoMonthsDelivery(deliveryDay);
			}
			if (DeliveryOncePerMonthCheckBox.IsChecked == true)
			{
				var deliveryDay = new DeliveryDay(DeliveryOncePerMonthComboBox.SelectedIndex + 1);
				return new OncePerMonthDelivery(deliveryDay);
			}
			if (DeliveryTwicePerMonthCheckBox.IsChecked == true)
			{
				var deliveryFirstDay = new DeliveryDay(DeliveryTwicePerMonthComboBox.SelectedIndex + 1);
				var deliverySecondDay = new DeliveryDay(DeliveryTwicePerMonthComboBox2.SelectedIndex + 1);
				return new TwicePerMonthDelivery(deliveryFirstDay,
					deliverySecondDay);
			}
			return new SuspendedDelivery();
		}

		private IProductInfo ParseProductInfo()
		{
			if (RazorAndGelCheckBox.IsChecked == true)
			{
				return new ProductInfo(ProductTypes.RazorAndGel, RazorAndGelPrice);
			}
			if (RazorAndGelAndFoamCheckBox.IsChecked == true)
			{
				return new ProductInfo(ProductTypes.RazorAndGelAndFoam, RazorAndGelAndFoamPrice);
			}
			return new ProductInfo(ProductTypes.Razor, RazorPrice);
		}
	}
}
