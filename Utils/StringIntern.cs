namespace QLang.Utils
{
	internal static class StringIntern
	{
		private static List<string>[]? _internBuffer;


		public static string Intern(ReadOnlySpan<char> originalString)
		{
			if (originalString.Length == 0)
				return string.Empty;

			if (_internBuffer is null)
			{
				_internBuffer = new List<string>[32];
				for (int i = 0; i < 32; i++)
					_internBuffer[i] = new List<string>();
			}

			var index = originalString[0] & 0b00011111;
			var internList = _internBuffer[index];

			foreach (var item in internList)
			{
				if (CompareByValue(item, originalString))
					return item;
			}

			var allocatedString = new string(originalString);
			internList.Add(allocatedString);
			return allocatedString;
		}


		private static bool CompareByValue(string item1, ReadOnlySpan<char> item2)
		{
			if (item1.Length != item2.Length)
				return false;

			for (int i = 0; i < item1.Length; i++)
			{
				if (item1[i] != item2[i])
					return false;
			}

			return true;
		}
	}
}