using System;
using System.Windows;
using System.Windows.Controls;
using RazorCore.Cash;
using RazorCore.Subscription;

namespace RazorWpf
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private readonly CashController _cashController;

		public MainWindow()
		{
			InitializeComponent();

			for (int i = 1; i <= 28; i++)
			{
				RazorComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelAndFoam1ComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
				RazorAndGelAndFoam2ComboBox.Items.Add(new ComboBoxItem { Content = i.ToString() });
			}

			RazorComboBox.SelectedIndex = 0;
			RazorAndGelComboBox.SelectedIndex = 0;
			RazorAndGelAndFoam1ComboBox.SelectedIndex = 0;
			RazorAndGelAndFoam2ComboBox.SelectedIndex = 14;

			var priceList = new PriceList(1, 9, 19);
			_cashController = new CashController(DateTime.Now.Date, priceList);

			DatePicker.SelectedDateChanged += DatePickerOnSelectedDateChanged;
		}

		private void DatePickerOnSelectedDateChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
		{
			_cashController.CurrentDate = DatePicker.DisplayDate;
			CashTxt.Text = ((int)_cashController.CalculateTotalCash()).ToString();
		}

		private void OnMakeSubscriptionClick(object sender, RoutedEventArgs e)
		{
			SubscriptionPlan subscriptionPlan = null;
			if (razorCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.Razor, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorComboBox.SelectedIndex + 1));
			}
			if (razorAndGelCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.RazorAndGel, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorAndGelComboBox.SelectedIndex + 1));
			}
			else if (razorAndGelAndFoamCheckBox.IsChecked == true)
			{
				subscriptionPlan = new SubscriptionPlan(SubscriptionTypes.RazorAndGelAndFoam, DeliveryRegularity.OncePerMonth,
					new DeliveryTime(RazorAndGelAndFoam1ComboBox.SelectedIndex + 1, RazorAndGelAndFoam2ComboBox.SelectedIndex + 1));
			}

			_cashController.SetSubscriptionPlan(subscriptionPlan);

			CashTxt.Text = ((int)_cashController.CalculateTotalCash()).ToString();
		}
	}
}
