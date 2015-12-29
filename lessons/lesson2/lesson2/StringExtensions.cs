using System;

namespace lesson2
{
	public static class StringExtensions
	{
		/// <summary>
		/// Returns the first maxLength characters of the string,
		/// or the string itself if it is shorter. 
		/// </summary>
		public static string Truncate(this string s, int maxLength) => (s == null || s.Length <= maxLength) ? s : s.Substring(0, maxLength);
	}
}

