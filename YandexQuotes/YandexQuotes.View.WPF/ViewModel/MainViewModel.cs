namespace YandexQuotes.View.WPF.ViewModel
{
	using System.Linq;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Windows;

	using Abt.Controls.SciChart;
	using Abt.Controls.SciChart.Visuals.Axes;

	using YandexQuotes.Core;


	public class MainViewModel : BaseViewModel
	{
		public MainViewModel(IEnumerable<Quote> model)
		{
			this.Quotes = model.Select(_ => new QuoteViewModel(_)).ToArray();

			this.YAxesCollection = new AxisCollection(
				model
					.GroupBy(_ => _.Unit)
					.Select(_ => new NumericAxis()
					{
						Id = _.Key,
						AxisTitle = _.Key,
						AutoRange = AutoRange.Always,
						AxisAlignment = AxisAlignment.Left,
						Visibility = Visibility.Collapsed,
					})
			);

			this.SeriesCollection = new ObservableCollection<IChartSeriesViewModel>();

			foreach (var q in this.Quotes)
				q.PropertyChanged += (sender, e) =>
				{
					if (e.PropertyName.Equals("IsChecked"))
					{
						var axis = this.YAxesCollection.OfType<NumericAxis>().Single(_ => _.Id == q.Unit);

						if (q.IsChecked)
						{
							axis.Visibility = Visibility.Visible;

							this.SeriesCollection.Add(q.Series);
						}
						else
						{
							this.SeriesCollection.Remove(q.Series);

							if (this.SeriesCollection.All(_ => _.RenderSeries.YAxis.Id != q.Unit))
								axis.Visibility = Visibility.Collapsed;
						}
					}
				};
		}

		public IEnumerable<QuoteViewModel> Quotes { get; private set; }

		public ObservableCollection<IChartSeriesViewModel> SeriesCollection { get; private set; }
		public AxisCollection YAxesCollection { get; private set; }
	}
}
