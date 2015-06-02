namespace YandexQuotes.Core
{
	using System;

	
	internal static class Extensions
	{
		public static DateTime UnixToDateTime(this long @this)
		{
			return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(@this);
		}
	}
}
