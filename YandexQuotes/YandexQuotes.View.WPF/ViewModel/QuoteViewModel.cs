namespace YandexQuotes.View.WPF.ViewModel
{
	using System;
	using System.Linq;

	using Abt.Controls.SciChart;
	using Abt.Controls.SciChart.Model.DataSeries;
	using Abt.Controls.SciChart.Visuals.RenderableSeries;

	using YandexQuotes.Core;


	public class QuoteViewModel : BaseViewModel
	{
		private readonly Quote model;

		private bool isChecked;

		public QuoteViewModel(Quote model)
		{
			this.model = model;

			var dataSeries = new XyDataSeries<DateTime, double>() { SeriesName = this.Name };

			this.Series = new ChartSeriesViewModel(
				dataSeries,
				new FastLineRenderableSeries()
				{
					SeriesColor = ColorFactory.Random(),
					YAxisId = this.Unit,
				}
			);

			this.model.DataChanged += () =>
			{
				using (dataSeries.SuspendUpdates())
				{
					dataSeries.Clear();
					dataSeries.Append(
						this.model.Data.Select(_ => _.Key),
						this.model.Data.Select(_ => _.Value)
					);
				}
			};
		}

		public string Name
		{
			get
			{
				return this.model.Name;
			}
		}
		public string Unit
		{
			get
			{
				return this.model.Unit;
			}
		}

		public bool IsChecked
		{
			get
			{
				return this.isChecked;
			}
			set
			{
				if (this.isChecked != value)
				{
					this.isChecked = value;

					this.CallPropertyChanged("IsChecked");

					if (this.isChecked)
						this.model.RefreshData();
				}
			}
		}

		public IChartSeriesViewModel Series { get; private set; }
	}
}
