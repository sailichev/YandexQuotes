namespace YandexQuotes.Core
{
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Xml.Linq;


	public static class QuoteService
	{
		public static IEnumerable<Quote> List(string fileName)
		{
			if (File.Exists(fileName))
			{
				// todo : here goes schema validation as well only the schema is yet to be created
			}
			else
			{
				return Enumerable.Empty<Quote>();
			}

			var quotes = XDocument.Load(fileName).Root;

			var urlFormat = quotes.Attribute("url-format").Value;

			return quotes.Elements("series")
				.Where(_ => bool.Parse(_.Attribute("enabled").Value))
				.Select(_ =>
					new Quote(
						int.Parse(_.Attribute("id").Value),
						_.Attribute("name").Value,
						_.Attribute("unit").Value,
						string.Format(urlFormat, int.Parse(_.Attribute("id").Value))
					)
				);			
		}
	}
}
