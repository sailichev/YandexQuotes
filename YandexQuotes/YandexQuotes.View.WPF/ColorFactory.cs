namespace YandexQuotes.View.WPF
{
	using System;
	using System.Windows.Media;


	public static class ColorFactory
	{
		private readonly static Random random = new Random();

		public static Color Random()
		{
			return Color.FromRgb(
				(byte)random.Next(0, 200),
				(byte)random.Next(0, 200),
				(byte)random.Next(0, 200)
			);
		}
	}
}
