namespace QLang.Tokenization;

internal class TokenizationException : Exception
{
	public TokenizationException(string rawText, Token.TextPosition currentPosition) : base(CreateMessage(rawText, currentPosition))
	{
		CurrentPosition = currentPosition;
	}


	public Token.TextPosition CurrentPosition { get; }


	private static string CreateMessage(string rawText, Token.TextPosition currentPosition)
	{
		const int charsToPrint = 25;

		var raw = rawText.AsSpan(currentPosition.AbsoluteIndex);

		string fancyErrorMessage;
		if (raw.Length >= charsToPrint)
		{
			fancyErrorMessage = new string(raw[..(charsToPrint)]);
			fancyErrorMessage += "...";
		}
		else
		{
			fancyErrorMessage = new string(raw);
		}

		fancyErrorMessage = fancyErrorMessage.Replace("\n", "\\n").Replace("\t", "\\t").Replace("\r", "");
		return fancyErrorMessage;
	}
}