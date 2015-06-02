namespace YandexQuotes.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Globalization;


	public class Quote
	{
		private readonly int id;
		private readonly string dataUrl;
	
		public string Name { get; private set; }
		public string Unit { get; private set; }
		public IEnumerable<KeyValuePair<DateTime, double>> Data { get; private set; }

		public event Action DataChanged;

		public async Task RefreshData(bool force = false)
		{
			if (this.Data == null || force)
				using (var httpClient = new HttpClient())
				{
					var xml = await httpClient.GetStringAsync(this.dataUrl);
					this.Data = ParseQuoteXml(xml);

					if (this.DataChanged != null)
						this.DataChanged();
				}	
		}

		internal Quote(int id, string name, string unit, string dataUrl)
		{
			this.id = id;
			this.dataUrl = dataUrl;
			this.Name = name;
			this.Unit = unit;
		}

		private static IEnumerable<KeyValuePair<DateTime, double>> ParseQuoteXml(string xml)
		{
			var series = XDocument.Parse(xml).Root;

			IFormatProvider culture = new CultureInfo("en-US");

			var x = series.Element("x").Value.Split(';')
				.Select(long.Parse)
				.Select(_ => _.UnixToDateTime())
				.ToArray();

			var y = series.Element("y").Value.Split(';')
				.Select(_ => double.Parse(_, culture))
				.ToArray();

			ICollection<KeyValuePair<DateTime, double>> res = new List<KeyValuePair<DateTime, double>>(y.Length);

			for (int i = 0; i < y.Length; i++)
				res.Add(new KeyValuePair<DateTime, double>(x[i], y[i]));

			return res;
		}
	}
}
