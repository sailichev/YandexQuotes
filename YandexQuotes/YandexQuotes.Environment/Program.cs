namespace Environment
{
	using System;
	using System.Windows;

	using YandexQuotes.Core;
	using YandexQuotes.View.WPF;
	using YandexQuotes.View.WPF.ViewModel;


	public class Program
	{
		[STAThread]
		public static void Main()
		{
			var model = QuoteService.List("yandex-quotes.xml");
			var viewModel = new MainViewModel(model);
			var view = new MainWindow() { DataContext = viewModel };

			new Application().Run(view);
		}
	}
}
