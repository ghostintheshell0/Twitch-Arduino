namespace HelloArduino
{
	public static class StringExtensions
	{
		public static string GetFirstWord(this string s)
		{
			if (s.Contains(" "))
			{
				var firstSpaceIndex = s.IndexOf(" ");
				return s.Substring(0, firstSpaceIndex);
			}
			return s;
		}
	}
}
