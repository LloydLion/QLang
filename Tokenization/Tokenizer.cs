namespace QLang.Tokenization;

internal sealed class Tokenizer : ITokenizer
{
	private readonly ITokenParser _tokenParser;


	public Tokenizer(ITokenParser tokenParser)
	{
		_tokenParser = tokenParser;
	}


	public TokenSequence Tokenize(string rawText)
	{
		var raw = rawText.AsSpan();
		var result = new List<Token>();
		var currentPosition = new Token.TextPosition(1, 0, 0);

		while (raw.Length != 0)
		{
			if (_tokenParser.TryParse(raw, out var token, out var charAte, currentPosition))
			{
				result.Add(token.Value);
				ShiftText(ref raw, charAte, ref currentPosition);
			}
			else
			{
				throw new TokenizationException(rawText, currentPosition);
			}
		}

		return new TokenSequence(result);
	}


	private static void ShiftText(ref ReadOnlySpan<char> rawText, int offset, ref Token.TextPosition currentPosition)
	{
		var omitted = rawText[..offset];
		rawText = rawText[offset..];

		var newLines = omitted.Count('\n');
		var lastLineLength = omitted[(omitted.LastIndexOf('\n') + 1)..].Length; //Not found case - OK

		var cpLine = currentPosition.Line + newLines;
		var cpAbsoluteIndex = currentPosition.AbsoluteIndex + offset;
		int cpCharacter;
		if (newLines == 0)
			cpCharacter = currentPosition.Character + lastLineLength;
		else
			cpCharacter = lastLineLength;

		currentPosition = new Token.TextPosition(cpLine, cpCharacter, cpAbsoluteIndex);
	}
}