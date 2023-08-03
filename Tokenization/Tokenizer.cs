using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace QLang.Tokenization;

internal sealed class Tokenizer : ITokenizer
{
	private readonly IEnumerable<ITokenParser> _tokenParsers;


	public Tokenizer(IEnumerable<ITokenParser> tokenParsers)
	{
		_tokenParsers = tokenParsers;
	}


	public TokenSequence Tokenize(string rawText)
	{
		var raw = rawText.AsSpan();
		var result = new List<Token>();
		var currentPosition = new Token.TextPosition(1, 0);

		while (raw.Length != 0)
		{
			if (TryParse(raw, out var token, out var charAte, currentPosition))
			{
				result.Add(token.Value);
				ShiftText(ref raw, charAte, ref currentPosition);
			}
			else
			{
				ThrowError(raw, currentPosition);
			}
		}

		return new TokenSequence(result);
	}

	private bool TryParse(ReadOnlySpan<char> raw, [NotNullWhen(true)] out Token? result, [NotNullWhen(true)] out int charAte, Token.TextPosition textPosition = default)
	{
		foreach (var tokenType in _tokenParsers)
		{
			if (tokenType.TryParse(raw, out result, out charAte, textPosition))
				return true;
			else continue;
		}

		result = null;
		charAte = 0;
		return false;
	}

	private static void ShiftText(ref ReadOnlySpan<char> rawText, int offset, ref Token.TextPosition currentPosition)
	{
		var omitted = rawText[..offset];
		rawText = rawText[offset..];

		var newLines = omitted.Count('\n');
		var lastLineLength = omitted[(omitted.LastIndexOf('\n') + 1)..].Length; //Not found case - OK

		var cpLine = currentPosition.Line + newLines;
		int cpCharacter;
		if (newLines == 0)
			cpCharacter = currentPosition.Character + lastLineLength;
		else
			cpCharacter = lastLineLength;

		currentPosition = new Token.TextPosition(cpLine, cpCharacter);
	}
	
	private static void ThrowError(ReadOnlySpan<char> raw, Token.TextPosition currentPosition)
	{
		string fancyErrorMessage;
		if (raw.Length >= 15)
		{
			fancyErrorMessage = new string(raw[..(15)]);
			fancyErrorMessage += "...";
		}
		else
		{
			fancyErrorMessage = new string(raw);
		}

		fancyErrorMessage = fancyErrorMessage.Replace("\n", "\\n");

		throw new TokenizationException($"Enable to tokenize this sequence: {currentPosition} HERE -> {fancyErrorMessage}");
	}
}